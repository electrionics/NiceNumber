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
    public class CheckService:ICheckService
    {
        private readonly NumberDataContext _dbContext;

        public CheckService(NumberDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Check> CheckRegularity(Guid gameId, string sessionId, byte[] positions, RegularityType type)
        {
            var regularitiesToCheck = await _dbContext.Set<Regularity>()
                .Where(x =>
                    x.Number.Games.Any(y => y.Id == gameId && y.SessionId == sessionId) &&
                    x.Type == type)
                .ToListAsync();

            var regularityToCheck = regularitiesToCheck
                .Select(r => new Tuple<int, int, Regularity>(
                                 r.AllPositions.Count(pos => !positions.Contains(pos)), 
                                 positions.Count(pos => !r.AllPositions.Contains(pos)), 
                                 r))
                .OrderByDescending(t => t.Item1 + t.Item2)
                .FirstOrDefault();

            if (regularityToCheck == null)
            {
                return null;
            }

            var check = new Check
            {
                CheckType = type,
                GameId = gameId,
                ClosestRegularity = regularityToCheck.Item3,
                NeedAddDigits = regularityToCheck.Item1,
                NeedRemoveDigits = regularityToCheck.Item2,
                TimePerformed = DateTime.Now
            };

            check.Regularity = regularityToCheck.Item1 + regularityToCheck.Item2 == 0
                ? check.ClosestRegularity
                : null;
            
            check.ScoreAdded = check.Regularity == null
                ? 0
                : 10; //TODO: need algorithm

            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                _dbContext.Set<Check>().Add(check);
                if (check.ScoreAdded > 0)
                {
                    var game = _dbContext.Set<Game>().First(x => x.Id == gameId && x.SessionId == sessionId);
                    game.Score += check.ScoreAdded;
                }

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            
            return check;
        }
    }
}