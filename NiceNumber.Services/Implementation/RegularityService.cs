using System.Threading.Tasks;
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
        
        public async Task<Regularity> SaveRegularity(Regularity regularity)
        {
            _dataContext.Set<Regularity>().Add(regularity);
            await _dataContext.SaveChangesAsync();
            return regularity;
        }
    }
}