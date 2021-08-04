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

        public async Task<CheckResult> CheckRegularity(Guid gameId, string sessionId, byte[] positions, RegularityType type)
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
                .OrderBy(t => t.Item1 + t.Item2)
                .FirstOrDefault();

            if (regularityToCheck == null)
            {
                return null;
            }

            var check = new Check
            {
                CheckType = type,
                GameId = gameId,
                CheckPositions = string.Join(',', positions),
                ClosestRegularity = regularityToCheck.Item3,
                NeedAddDigits = regularityToCheck.Item1,
                NeedRemoveDigits = regularityToCheck.Item2,
                TimePerformed = DateTime.Now
            };

            check.Regularity = regularityToCheck.Item1 + regularityToCheck.Item2 == 0
                ? check.ClosestRegularity
                : null;

            check.RegularityId = check.Regularity?.Id;
            check.ClosestRegularityId = check.ClosestRegularity.Id;

            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                var existCheck = await _dbContext.Set<Check>()
                    .FirstOrDefaultAsync(x => x.RegularityId == check.RegularityId);
            
                check.ScoreAdded = check.Regularity == null || existCheck != null
                    ? 0
                    : 10; //TODO: need algorithm
                
                _dbContext.Set<Check>().Add(check);
                var game = _dbContext.Set<Game>().First(x => x.Id == gameId && x.SessionId == sessionId);
                check.Game = game;
                
                if (check.ScoreAdded > 0)
                {
                    game.Score += check.ScoreAdded;
                }

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            
            return new CheckResult { Value = check, RegularityNumber = check.Regularity?.RegularityNumber ?? 0};
        }
    }
}