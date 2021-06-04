using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
            int length, minRegCount, maxRegCount;
            switch (level)
            {
                case DifficultyLevel.Easy:
                    length = 8;
                    minRegCount = 5;
                    maxRegCount = 10;
                    break;
                case DifficultyLevel.Normal:
                    length = 10;
                    minRegCount = 10;
                    maxRegCount = 20;
                    break;
                case DifficultyLevel.Hard:
                    length = 12;
                    minRegCount = 20;
                    maxRegCount = 30;
                    break;
                default:
                    throw new ArgumentException($@"Invalid difficulty level. Value: {level}.");
            }

            var limit = await _dbContext.Set<Number>().CountAsync(x => 
                x.Length == length && 
                x.Regularities.Count <= maxRegCount &&
                x.Regularities.Count >= minRegCount) - 1;
            
            var generator = new Random();
            var skipCount = generator.Next(limit);

            var number = await _dbContext.Set<Number>()
                    .Include(x => x.Regularities)
                .Where(x => 
                        x.Length == length && 
                        x.Regularities.Count <= maxRegCount &&
                        x.Regularities.Count >= minRegCount)
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

        public async Task<Game> EndGame(Guid gameId, string sessionId, bool inBackground = false)
        {
            var game = _dbContext.Set<Game>()
                .Include(x => x.Checks).ThenInclude(x => x.Regularity)
                .FirstOrDefault(x => x.SessionId == sessionId && x.Id == gameId);

            if (game != null)
            {
                if (game.FinishTime == null)
                {
                    game.FinishTime = DateTime.Now;
                }
                game.Score = game.Checks.Sum(x => x.ScoreAdded);
                game.EndInBackground = inBackground;
                
                await _dbContext.SaveChangesAsync();
            }

            return game;
        }
    }
}