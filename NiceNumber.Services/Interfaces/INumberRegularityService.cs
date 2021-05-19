using System.Threading.Tasks;
using NiceNumber.Core;
using NiceNumber.Domain.Entities;

namespace NiceNumber.Services.Interfaces
{
    public interface INumberRegularityService
    {
        Task<int> GetCountOfNumbers(int numberLength);

        Game StartRandomNumberGame(string sessionId);

        Check CheckRegularity(string sessionId, byte[] positions, RegularityType type);
    }
}