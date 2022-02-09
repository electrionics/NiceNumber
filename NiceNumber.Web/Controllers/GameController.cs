using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NiceNumber.Core;
using NiceNumber.Domain.Entities;
using NiceNumber.Services.Interfaces;
using NiceNumber.Web.Filters;
using NiceNumber.Web.Results;
using NiceNumber.Web.ViewModels;

namespace NiceNumber.Web.Controllers
{
    [ApiController]
    public class GameController:ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly ICheckService _checkService;
        private readonly ITutorialService _tutorialService;

        private string SessionId => HttpContext.Session.GetString(SessionIdFilter.CookieSessionForGameKey);

        public GameController(IGameService gameService, ICheckService checkService, ITutorialService tutorialService)
        {
            _gameService = gameService;
            _checkService = checkService;
            _tutorialService = tutorialService;
        }

        #region Start
        
        [HttpGet]
        [ApiRoute("Game/Start")]
        public async Task<StartModel> Start([FromQuery]DifficultyLevel difficultyLevel, [FromQuery]int? tutorialLevel)
        {
            var sessionId = SessionId;

            var game = difficultyLevel == DifficultyLevel.Tutorial && tutorialLevel.HasValue
                ? await _gameService.StartTutorial(tutorialLevel.Value, sessionId)
                : await _gameService.StartRandomNumberGame(difficultyLevel, sessionId);

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
                }).OrderBy(x => x.Type)
                    .ThenBy(x => x.RegularityNumber)
                    .ToList(),
                ExistRegularityTypeCounts = new Dictionary<int, int>(),
                
                TutorialLevel = game.Number.TutorialLevel == null 
                    ? null 
                    : new TutorialLevelModel
                        {
                            Title = game.Number.TutorialLevel.Title,
                            Text = game.Number.TutorialLevel.Text,
                            Level = game.Number.TutorialLevel.Level,
                            Tasks = game.Number.TutorialLevel.Tasks.OrderBy(x => x.Order).Select(x => new TutorialTaskModel
                            {
                                Name = x.Name,
                                Text = x.Text,
                                Order = x.Order,
                                AnySubtask = x.AnySubtask,
                                Subtasks = x.Subtasks?.Split(',').Select(int.Parse).ToList(),
                                ApplyCondition = x.ApplyCondition,
                                ConditionParameter = x.ConditionParameter
                            }).ToList()
                        }
            };

            foreach (var regularityInfo in model.ExistRegularityInfos)
            {
                if (regularityInfo.RegularityNumber is > 0 and < 1)
                {
                    regularityInfo.ReverseRegularityNumber = Math.Round(1 / regularityInfo.RegularityNumber, 0);
                }
            }

            var regularityTypeCounts = game.Number.Regularities
                .GroupBy(x => x.Type)
                .ToDictionary(
                    x => x.Key,
                    x => x.Count());
            foreach (var type in Enum.GetValues<RegularityType>())
            {
                model.ExistRegularityTypeCounts[(int) type] = regularityTypeCounts.TryGetValue(type, out var count)
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
            var sessionId = SessionId;
            var hinted = HttpContext.Session.TryGetValue(NextCheckIsHintSessionKey + data.GameId, out _);
            
            var check = await _checkService.CheckRegularity(data.GameId, sessionId, data.Positions.Select(x => (byte)x).ToArray(), data.Type, hinted);

            if (hinted)
            {
                HttpContext.Session.Remove(NextCheckIsHintSessionKey + data.GameId);
            }
            
            if (check == null)
            {
                return null;
            }

            var model = PrepareModel(check, hinted);
            return model;
        }

        private static CheckResultModel PrepareModel(CheckResult entity, bool hinted)
        {
            var check = entity.Value;
            
            var model = new CheckResultModel
            {
                Match = check.RegularityId != null,
                PointsAdded = check.ScoreAdded,
                NewTotalPoints = check.Game.Score,
                AddHint = CheckHint.No,
                RemoveHint = CheckHint.No,
                RegularityNumber = entity.RegularityNumber,
                Hinted = hinted,
                FoundNumbers = check.Regularity?.StartPositions.Select((pos, i) => new FoundNumberModel
                {
                    Position = pos,
                    Length = check.Regularity.SubNumberLengths[i]
                }).ToArray()
            };
            
            if (check.NeedAddDigits == 1)
            {
                model.AddHint = CheckHint.AddOneDigit;
            }
            else if (check.NeedAddDigits > 1)
            {
                model.AddHint = CheckHint.AddMoreThanOneDigit;
            }
            
            if (check.NeedRemoveDigits == 1)
            {
                model.RemoveHint = CheckHint.RemoveOneDigit;
            }
            else if (check.NeedRemoveDigits > 1)
            {
                model.RemoveHint = CheckHint.RemoveMoreThanOneDigit;
            }

            return model;
        }

        #endregion
        
        
        #region Hint

        private const string NextCheckIsHintSessionKey = "NextCheckIsHint";

        [HttpPost]
        [ApiRoute("Game/Hint")]
        public async Task<HintResultModel> Hint(HintModel data)
        {
            var sessionId = SessionId;

            var hint = await _checkService.GetRandomCheck(data.GameId, sessionId, data.Type, data.RegularityNumber);
            if (hint != null)
            {
                HttpContext.Session.Set(NextCheckIsHintSessionKey + data.GameId, Array.Empty<byte>());

                return new HintResultModel
                {
                    Numbers = hint.StartPositions.Select((pos, i) => new FoundNumberModel
                    {
                        Position = pos,
                        Length = hint.SubNumberLengths[i]
                    }).ToArray(),
                    Type = hint.Type,
                    RegularityNumber = hint.RegularityNumber
                };
            }

            return null;
        }
        
        #endregion


        #region Records

        [HttpGet]
        [ApiRoute("Game/Records")]
        public async Task<List<RecordModel>> Records(int? days, DifficultyLevel? difficultyLevel)
        {
            const int topRecords = 15;
            var possibleDays = new int?[] {null, 1, 7, 30};
            var sessionId = SessionId;
            
            if (!possibleDays.Contains(days))
            {
                days = null;
            }

            if (difficultyLevel != null && (!Enum.GetValues<DifficultyLevel>().Contains(difficultyLevel.Value) || difficultyLevel == 0))
            {
                difficultyLevel = null;
            }

            var games = await _gameService.GetTopResults(days, difficultyLevel, topRecords);
            var model = games.Select((game, i) => new RecordModel
            {
                Position = i + 1,
                CurrentPlayer = game.SessionId == sessionId,
                Score = game.Score,
                Player = game.PlayerName ?? "Игрок",
                Link = game.PlayerLink
            }).ToList();

            return model;
        }

        #endregion
        
        
        #region End

        [HttpPost]
        [ApiRoute("Game/End")]
        public async Task<EndModel> End([FromQuery] Guid gameId, [FromQuery] int remainingSeconds)
        {
            var sessionId = SessionId;

            var game = await _gameService.EndGame(gameId, sessionId, remainingSeconds);

            if (game == null)
            {
                return null;
            }

            var foundAndHintedRegularityIds = game.Checks
                .Where(y => y.RegularityId != null)
                .Select(y => (int)y.RegularityId)
                .ToHashSet();
            var notFoundRegularities = game.Number.Regularities
                .Where(x => x.Playable && !foundAndHintedRegularityIds.Contains(x.Id))
                .ToList();
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
                        Count = x.Count(y => y.Status == CheckStatus.Match)
                    }).ToList(),
                NotFoundRegularityInfos = notFoundRegularities.Select(x => new HintResultModel
                {
                    Numbers = x.StartPositions.Select((pos, i) => new FoundNumberModel
                    {
                        Position = pos,
                        Length = x.SubNumberLengths[i]
                    }).ToArray(),
                    RegularityNumber = x.RegularityNumber,
                    Type = x.Type
                }).ToList()
            };

            return model;
        }

        #endregion


        #region UpdateEnded

        [HttpPost]
        [ApiRoute("Game/UpdateEnded")]
        public async Task<Result> UpdateEnded(UpdateEndedModel model)
        {
            if (ModelState.IsValid)
            {
                var sessionId = SessionId;

                return new Result
                {
                    Success = await _gameService.UpdateEndedGame(model.GameId, sessionId, model.Name, model.Link),
                    Errors = new List<KeyValuePair<string, string>>()
                };
            }

            return new Result
            {
                Success = false,
                Errors = ModelState.SelectMany(x =>
                        x.Value.Errors.Select(e => new KeyValuePair<string, string>(x.Key, e.ErrorMessage)))
                    .ToList()
            };
        }

        #endregion


        #region TutorialDetails

        
        [HttpGet]
        [ApiRoute("Game/TutorialDetails")]
        public async Task<TutorialDetailsModel> GetTutorialDetails()
        {
            var (levelsCount, tasksCount) = await _tutorialService.GetTutorialCounts();

            var model = new TutorialDetailsModel
            {
                TotalLevels = levelsCount,
                TotalTasks = tasksCount
            };
            
            return model;
        }

        #endregion


        #region Personal Records

        [HttpGet]
        [ApiRoute("Game/PersonalRecords")]
        public async Task<List<PersonalRecordModel>> PersonalRecords(int? days)
        {
            var possibleDays = new int?[] { null, 1, 7, 30 };
            var sessionId = SessionId;

            if (!possibleDays.Contains(days))
            {
                days = null;
            }

            var games = await _gameService.GetPresonalTop1Results(days, sessionId);
            var model = games.Select(game => new PersonalRecordModel
            {
                Score = game.Score,
                Player = game.PlayerName ?? "Игрок",
                DifficultyLevel = game.DifficultyLevel
            }).OrderBy(x => x.DifficultyLevel).ToList();

            return model;
        }

        #endregion
    }
}