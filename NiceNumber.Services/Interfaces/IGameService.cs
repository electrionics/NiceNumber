using System;
using System.Threading.Tasks;
using NiceNumber.Domain.Entities;

namespace NiceNumber.Services.Interfaces
{
    public interface IGameService
    {
        Task<Game> StartRandomNumberGame(DifficultyLevel level, string sessionId);
        Task<Game> EndGame(Guid gameId, string sessionId, bool inBackground = false);
    }
}