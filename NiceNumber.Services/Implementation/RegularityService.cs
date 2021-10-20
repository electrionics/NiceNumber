using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NiceNumber.Domain;
using NiceNumber.Domain.Entities;
using NiceNumber.Services.Interfaces;

namespace NiceNumber.Services.Implementation
{
    public class RegularityService:IRegularityService
    {
        private readonly NumberDataContext _dataContext;

        public RegularityService(NumberDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Regularity>> GetAllRegularities()
        {
            return await _dataContext.Set<Regularity>().ToListAsync();
        }

        public async Task<Regularity> SaveRegularity(Regularity regularity)
        {
            if (regularity.Id == 0)
            {
                _dataContext.Set<Regularity>().Add(regularity);
            }
            
            await _dataContext.SaveChangesAsync();
            return regularity;
        }

        public async Task SaveChanges()
        {
            await _dataContext.SaveChangesAsync();
        }

        public async Task RemoveRegularitiesMarkedAsDeleted()
        {
            var toDelete = _dataContext.Set<Regularity>().Where(x => x.Deleted && !x.Checks.Any() && !x.ClosestChecks.Any());
            _dataContext.Set<Regularity>().RemoveRange(toDelete);
            await _dataContext.SaveChangesAsync();
        }
    }
}