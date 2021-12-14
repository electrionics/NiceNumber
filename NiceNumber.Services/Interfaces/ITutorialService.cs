using System;
using System.Threading.Tasks;

namespace NiceNumber.Services.Interfaces
{
    public interface ITutorialService
    {
        Task<Tuple<int, int>> GetTutorialCounts();
    }
}