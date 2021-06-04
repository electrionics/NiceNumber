using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NiceNumber.Core;
using NiceNumber.Domain.Entities;
using NiceNumber.Services.Interfaces;
using NiceNumber.Web.ViewModels;

namespace NiceNumber.Web.Controllers
{
    [ApiController]
    public class GameController:ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly ICheckService _checkService;

        public GameController(IGameService gameService, ICheckService checkService)
        {
            _gameService = gameService;
            _checkService = checkService;
        }

        #region Test

        [HttpGet]
        [ApiRoute("Game/Test")]
        public async Task<int> Test()
        {
            return 1;
        }

        #endregion

        #region Start
        
        [HttpGet]
        [ApiRoute("Game/Start")]
        public async Task<StartModel> Start([FromQuery]DifficultyLevel difficultyLevel)
        {
            var sessionId = HttpContext.Session.Id;
            
            var game = await _gameService.StartRandomNumberGame(difficultyLevel, sessionId);

            var model = new StartModel
            {
                GameId = game.Id,
                Number = game.Number.Value,
                Length = game.Number.Length,
                DifficultyLevel = difficultyLevel,
                ExistRegularityInfos = game.Number.Regularities.Select(x => new StartRegularityInfo
                {
                    RegularityNumber = x.RegularityNumber,
                    Type = x.Type,
                }).ToList(),
                ExistRegularityTypeCounts = new Dictionary<RegularityType, int>()
            };
            
            var regularityTypeCounts = game.Number.Regularities
                .GroupBy(x => x.Type)
                .ToDictionary(
                    x => x.Key,
                    x => x.Count());
            foreach (var type in Enum.GetValues<RegularityType>())
            {
                model.ExistRegularityTypeCounts[type] = regularityTypeCounts.TryGetValue(type, out var count)
                    ? count
                    : 0;
            }

            return model;
        }

        #endregion


        #region Check

        [HttpPost]
        [ApiRoute("Game/Check")]
        public async Task<CheckResultModel> Check(CheckModel data)
        {
            var sessionId = HttpContext.Session.Id;

            var check = await _checkService.CheckRegularity(data.GameId, sessionId, data.Positions, data.Type);
            
            var model = PrepareModel(check);
            return model;
        }

        private static CheckResultModel PrepareModel(Check entity)
        {
            var model = new CheckResultModel
            {
                Match = entity.NeedAddDigits == 0,
                PointsAdded = entity.ScoreAdded,
                NewTotalPoints = entity.Game.Score,
                AddHint = CheckHint.No,
                RemoveHint = CheckHint.No
            };
            
            if (entity.NeedAddDigits == 1)
            {
                model.AddHint = CheckHint.AddOneDigit;
            }
            else if (entity.NeedAddDigits > 1)
            {
                model.AddHint = CheckHint.AddMoreThanOneDigit;
            }
            
            if (entity.NeedRemoveDigits == 1)
            {
                model.AddHint = CheckHint.RemoveOneDigit;
            }
            else if (entity.NeedRemoveDigits > 1)
            {
                model.AddHint = CheckHint.RemoveMoreThanOneDigit;
            }

            return model;
        }

        #endregion

        
        #region End

        [HttpPost]
        [ApiRoute("Game/End")]
        public async Task<EndModel> End([FromBody] Guid gameId)
        {
            var sessionId = HttpContext.Session.Id;

            var game = await _gameService.EndGame(gameId, sessionId);

            if (game == null)
            {
                return null;
            }

            // ReSharper disable once PossibleInvalidOperationException
            var spentTime = game.FinishTime.Value - game.StartTime;
            var model = new EndModel
            {
                TotalScore = game.Score,
                SpentMinutes = Convert.ToInt32(Math.Floor(spentTime.TotalMinutes)),
                SpentSeconds = spentTime.Seconds,
                FoundRegularityInfos = game.Checks
                    .GroupBy(x => x.CheckType)
                    .Select(x => new EndRegularityInfo
                    {
                        Type = x.Key,
                        Count = x.Count(y => y.RegularityId != null)
                    }).ToList()
            };

            return model;
        }

        #endregion
    }
}