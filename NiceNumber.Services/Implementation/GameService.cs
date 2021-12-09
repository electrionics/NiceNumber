using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NiceNumber.Core;
using NiceNumber.Domain;
using NiceNumber.Domain.Entities;
using NiceNumber.Services.Interfaces;

namespace NiceNumber.Services.Implementation
{
    public class GameService: IGameService
    {
        private readonly NumberDataContext _dbContext;

        public GameService(NumberDataContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<Game> StartRandomNumberGame(DifficultyLevel level, string sessionId)
        {
            int minLength, maxLength, minRegCount, maxRegCount;
            switch (level)
            {
                case DifficultyLevel.Easy:
                    minLength = 7;
                    maxLength = 8;
                    minRegCount = 6;
                    maxRegCount = 10;
                    break;
                case DifficultyLevel.Normal:
                    minLength = 9;
                    maxLength = 10;
                    minRegCount = 11;
                    maxRegCount = 20;
                    break;
                case DifficultyLevel.Hard:
                    minLength = 11;
                    maxLength = 12;
                    minRegCount = 21;
                    maxRegCount = 30;
                    break;
                default:
                    throw new ArgumentException($@"Invalid difficulty level. Value: {level}.");
            }

            Expression<Func<Number, bool>> numberCondition = x =>
                x.Length >= minLength &&
                x.Length <= maxLength &&
                x.Regularities.Count(y => y.Playable) <= maxRegCount &&
                x.Regularities.Count(y => y.Playable) >= minRegCount;
            
            var limit = await _dbContext.Set<Number>().CountAsync(numberCondition) - 1;
            
            var generator = new Random();
            var skipCount = generator.Next(limit);

            var number = await _dbContext.Set<Number>()
                    .Include(x => x.Regularities.Where(y => y.Playable))
                .Where(numberCondition)
                .Skip(skipCount)
                .Take(1)
                .FirstOrDefaultAsync();

            if (number == null)
            {
                throw new InvalidOperationException("Supposed number was accidentially removed, please try again.");
            }
            
            var game = new Game
            {
                Id = Guid.NewGuid(),
                DifficultyLevel = level,
                NumberId = number.Id,
                Score = 0,
                SessionId = sessionId,
                StartTime = DateTime.Now,
                Number = number
            };

            _dbContext.Set<Game>().Add(game);
            await _dbContext.SaveChangesAsync();

            return game;
        }

        public async Task<Game> StartTutorial(int tutorialLevel, string sessionId)
        {
            var number = await _dbContext.Set<Number>()
                .Include(x => x.Regularities.Where(y => y.Playable))
                .Include(x => x.TutorialLevel).ThenInclude(x => x.Tasks)
                .FirstOrDefaultAsync(x => x.TutorialLevel != null && x.TutorialLevel.Level == tutorialLevel);

            if (number == null)
            {
                throw new InvalidOperationException("Supposed number was accidentially removed, please try again.");
            }
            
            var game = new Game
            {
                Id = Guid.NewGuid(),
                DifficultyLevel = DifficultyLevel.Tutorial,
                NumberId = number.Id,
                Score = 0,
                SessionId = sessionId,
                StartTime = DateTime.Now,
                Number = number
            };

            _dbContext.Set<Game>().Add(game);
            await _dbContext.SaveChangesAsync();

            return game;
        }

        public async Task<Game> EndGame(Guid gameId, string sessionId, int remainingSeconds, bool inBackground = false)
        {
            var game = await _dbContext.Set<Game>()
                .Include(x => x.Checks).ThenInclude(x => x.Regularity)
                .Include(x => x.Number).ThenInclude(x => x.Regularities)
                .FirstOrDefaultAsync(x => x.SessionId == sessionId && x.Id == gameId);

            if (game != null)
            {
                if (game.FinishTime == null)
                {
                    game.FinishTime = DateTime.Now;
                }

                var checkScore = game.Checks.Sum(x => x.ScoreAdded);
                game.Score = checkScore + Math.Min(checkScore / 2, remainingSeconds);
                game.EndInBackground = inBackground;
                
                await _dbContext.SaveChangesAsync();
            }

            return game;
        }

        public async Task<bool> UpdateEndedGame(Guid gameId, string sessionId, string name, string link)
        {
            var game = await _dbContext.Set<Game>()
                .FirstOrDefaultAsync(x => x.SessionId == sessionId && x.Id == gameId);

            if (game != null && 
                game.FinishTime != null && 
                game.Score > 0 && 
                game.PlayerName == null && 
                game.PlayerLink == null)
            {
                game.PlayerName = name;
                game.PlayerLink = link;

                try
                {
                    await _dbContext.SaveChangesAsync();

                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            return false;
        }

        public async Task<List<Game>> GetTopResults(int? days, DifficultyLevel? difficultyLevel, int count)
        {
            var query = _dbContext.Set<Game>()
                .Where(x => x.FinishTime != null && x.Score > 0);
            if (days != null)
            {
                var minFinishTime = DateTime.Today.AddDays(1 - days.Value);
                
                query = query.Where(x => x.FinishTime > minFinishTime);
            }

            if (difficultyLevel != null)
            {
                query = query.Where(x => x.DifficultyLevel == difficultyLevel);
            }

            var games = await query
                .OrderByDescending(x => x.Score)
                .ThenByDescending(x => x.FinishTime)
                .Take(count)
                .ToListAsync();

            foreach (var game in games)
            {
                if (string.IsNullOrWhiteSpace(game.PlayerName))
                {
                    game.PlayerName = "Игрок";
                }
            }

            return games;
        }
    }
}