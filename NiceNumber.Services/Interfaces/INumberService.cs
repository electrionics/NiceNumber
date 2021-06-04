using System.Threading.Tasks;
using NiceNumber.Domain.Entities;

namespace NiceNumber.Services.Interfaces
{
    public interface INumberService
    {
        Task<Number> SaveNumber(Number number);
    }
}