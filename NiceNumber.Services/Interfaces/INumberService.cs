using System.Collections.Generic;
using System.Threading.Tasks;
using NiceNumber.Core;
using NiceNumber.Domain.Entities;

namespace NiceNumber.Services.Interfaces
{
    public interface INumberService
    {
        Task<Number> SaveNumber(Number number);
        Task<List<Number>> GetNumbers(int length, List<RegularityType> typesToRetrieve);
    }
}