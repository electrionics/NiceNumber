using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NiceNumber.Core;
using NiceNumber.Domain;
using NiceNumber.Domain.Entities;
using NiceNumber.Services.Interfaces;

namespace NiceNumber.Services.Implementation
{
    public class NumberRegularityService:INumberRegularityService
    {
        private readonly NumberDataContext _dbContext;

        public NumberRegularityService(NumberDataContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public Task<int> GetCountOfNumbers(int numberLength)
        {
            return _dbContext.Set<Number>().CountAsync();
        }

        public Game StartRandomNumberGame(string sessionId)
        {
            var rnd = new Random();

            return new Game();
        }    

        public Check CheckRegularity(string sessionId, byte[] positions, RegularityType type)
        {
            return new Check();
        }
    }
}