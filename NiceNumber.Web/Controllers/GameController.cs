using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NiceNumber.Domain.Entities;
using NiceNumber.Services.Interfaces;
using NiceNumber.Web.ViewModels;

namespace NiceNumber.Web.Controllers
{
    [ApiController]
    public class GameController:ControllerBase
    {
        private readonly INumberRegularityService _numberRegularityService;

        public GameController(INumberRegularityService numberRegularityService)
        {
            _numberRegularityService = numberRegularityService;
        }

        #region Test

        [HttpGet]
        [ApiRoute("Game/Test")]
        public async Task<int> Test()
        {
            return await _numberRegularityService.GetCountOfNumbers(1) + 1;
        }

        #endregion

        #region Start
        
        [HttpGet]
        [ApiRoute("Game/Start")]
        public StartModel Start()
        {
            var sessionId = HttpContext.Session.Id;
            
            var entity = _numberRegularityService.StartRandomNumberGame(sessionId);

            var model = new StartModel
            {
                GameId = entity.Id,
                Number = entity.Number.Value,
                Length = entity.Number.Length,
                RegularityInfos = entity.Number.Regularities.Select(x => new StartRegularityInfo
                {
                    RegularityNumber = x.RegularityNumber,
                    Type = x.Type
                }).ToList(),
                RegularityTypeCounts = entity.Number.Regularities
                    .GroupBy(x => x.Type)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Count())
            };

            return model;
        }

        #endregion


        #region Check

        [HttpPost]
        [ApiRoute("Game/Check")]
        public CheckResultModel Check(CheckModel data)
        {
            var sessionId = HttpContext.Session.Id;

            var check = _numberRegularityService.CheckRegularity(sessionId, data.Positions, data.Type);
            
            var model = PrepareModel(check);
            return model;
        }

        private static CheckResultModel PrepareModel(Check entity)
        {
            var model = new CheckResultModel
            {
                Match = entity.NeedAddDigits == 0,
                PointsAdded = entity.ScoreAdded,
                NewTotalPoints = entity.Game.Score
            };
            
            if (entity.NeedAddDigits == 1)
            {
                model.Hint = CheckHint.AddOneDigit;
            }

            return model;
        }

        #endregion
    }
}