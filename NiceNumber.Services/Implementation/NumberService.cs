using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NiceNumber.Core;
using NiceNumber.Domain;
using NiceNumber.Domain.Entities;
using NiceNumber.Services.Interfaces;

namespace NiceNumber.Services.Implementation
{
    public class NumberService:INumberService
    {
        private readonly NumberDataContext _dataContext;

        public NumberService(NumberDataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
        public async Task<Number> SaveNumber(Number number)
        {
            if (number.Id == 0)
            {
                _dataContext.Set<Number>().Add(number);
            }
            
            await _dataContext.SaveChangesAsync();
            return number;
        }

        public async Task<List<Number>> GetNumbers(int length, List<RegularityType> typesToRetrieve)
        {
            var result = await _dataContext.Set<Number>()
                .Include(x => x.Regularities.Where(y => !y.Deleted && typesToRetrieve.Contains(y.Type)))
                .Where(x => 
                    x.Length == length && 
                    x.Regularities.Any(y => !y.Deleted && typesToRetrieve.Contains(y.Type)))
                .ToListAsync();

            return result;
        }
    }
}