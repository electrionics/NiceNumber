using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NiceNumber.Core;
using NiceNumber.Core.Helpers;
using NiceNumber.Core.Regularities;
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

        public async Task<CheckResult> CheckRegularity(Guid gameId, string sessionId, byte[] positions, RegularityType type, bool hinted)
        {
            var regularitiesToCheck = await _dbContext.Set<Regularity>()
                .Where(x =>
                    x.Number.Games.Any(y => y.Id == gameId && y.SessionId == sessionId) &&
                    !x.Checks.Any(y => y.GameId == gameId && y.Game.SessionId == sessionId && y.RegularityId != null) &&
                    x.Type == type && x.Playable)
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

            if (hinted)
            {
                _dbContext.Set<Check>().Add(check);
                var game = _dbContext.Set<Game>().First(x => x.Id == gameId && x.SessionId == sessionId);
                check.Game = game;
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    var existCheck = await _dbContext.Set<Check>()
                        .FirstOrDefaultAsync(x => x.RegularityId == check.RegularityId && x.GameId == gameId && x.Game.SessionId == sessionId);
            
                    check.ScoreAdded = check.Regularity == null || existCheck != null
                        ? 0
                        : CalculateRegularityFoundScore(check.Regularity);
                
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
            }
            
            return new CheckResult { Value = check, RegularityNumber = check.Regularity?.RegularityNumber ?? 0};
        }

        private static int CalculateRegularityFoundScore(Regularity regularity)
        {
            var regNumber = (int) Math.Abs(Math
                .Max(1 / regularity.RegularityNumber, regularity.RegularityNumber)
                .RoundTo(RegularityConstants.DoubleRegularityNumberAccuracy));
            var subNumbersCount = regularity.SubNumberLengths.Count;
            return regularity.Type switch
            {
                RegularityType.SameDigits => 10 + 5 * (regNumber - 2),
                RegularityType.SameNumbers => 20 + 10 * (regNumber - 2),
                RegularityType.MirrorDigits => 30 + 5 * (subNumbersCount - 4),
                RegularityType.MultiplesNumbers => 40 + 5 * regNumber,
                RegularityType.ArithmeticProgression => 10 + 5 * regNumber,
                RegularityType.GeometricProgression => 20 + 10 * regNumber,
                
                _ => throw new NotImplementedException()
            };
        }

        public async Task<HintResult> GetRandomCheck(Guid gameId, string sessionId, RegularityType? type, double? regularityNumber)
        {
            var regularityToHintQuery = _dbContext.Set<Regularity>()
                .Where(x =>
                    x.Number.Games.Any(y => y.Id == gameId && y.SessionId == sessionId) &&
                    !x.Checks.Any(y => y.GameId == gameId && y.Game.SessionId == sessionId && y.RegularityId != null) &&
                    x.Playable);
            
            if (type != null)
            {
                regularityToHintQuery = regularityToHintQuery.Where(x => x.Type == type);
            }
            if (regularityNumber != null)
            {
                regularityToHintQuery = regularityToHintQuery.Where(x => x.RegularityNumber == regularityNumber);
            }
            
            var candidatesToHint = await regularityToHintQuery
                .OrderBy(x => x.Type)
                .ThenBy(x => x.RegularityNumber)
                .ToListAsync();

            var random = new Random();
            var regularityToHint = candidatesToHint
                .Skip(random.Next(0, Math.Max(candidatesToHint.Count - 1, 0)))
                .FirstOrDefault();

            if (regularityToHint == null)
            {
                return null;
            }

            var result = new HintResult
            {
                Type = regularityToHint.Type,
                StartPositions = regularityToHint.StartPositions,
                SubNumberLengths = regularityToHint.SubNumberLengths,
                AllPositions = regularityToHint.AllPositions,
                RegularityNumber = regularityToHint.RegularityNumber
            };

            return result;
        }
    }
}