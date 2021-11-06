using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NiceNumber.Domain.Entities;

namespace NiceNumber.Services.Interfaces
{
    public interface IGameService
    {
        Task<Game> StartRandomNumberGame(DifficultyLevel level, string sessionId);
        Task<Game> EndGame(Guid gameId, string sessionId, int remainingSeconds, bool inBackground = false);
        Task<bool> UpdateEndedGame(Guid gameId, string sessionId, string name, string link);
        Task<List<Game>> GetTopResults(int? days, DifficultyLevel? difficultyLevel, int count);
    }
}