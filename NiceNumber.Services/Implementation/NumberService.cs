using System.Threading.Tasks;
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
            _dataContext.Set<Number>().Add(number);
            await _dataContext.SaveChangesAsync();
            return number;
        }
    }
}