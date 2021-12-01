using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NiceNumber.Core;
using NiceNumber.Domain.Entities;

namespace NiceNumber.Services.Interfaces
{
    public interface INumberService
    {
        Task<Number> SaveNumber(Number number);
        Task<List<Number>> GetNumbers(Expression<Func<Number, bool>> filterExpression, List<RegularityType> typesToRetrieve);
    }
}