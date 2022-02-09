using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NiceNumber.Core;
using NiceNumber.Domain.Entities;
using NiceNumber.Services.Interfaces;

namespace NiceNumber
{
    public static class AddRecords
    {
        public static DateTime MinDate = new DateTime(2022, 02, 08);
        private static double ProbabilityOfNoNamePlayer = 0.6; // no name ends with 5 or 0 in current version (while way of assigning points wouldn't be changed)
        
        private static List<Tuple<DifficultyLevel, int, int>> PointsProbabilityConfiguration = new()
        {
            new Tuple<DifficultyLevel, int, int>(DifficultyLevel.Easy, 50, 2),
            new Tuple<DifficultyLevel, int, int>(DifficultyLevel.Easy, 100, 4),
            new Tuple<DifficultyLevel, int, int>(DifficultyLevel.Easy, 200, 8),
            new Tuple<DifficultyLevel, int, int>(DifficultyLevel.Easy, 400, 8),

            new Tuple<DifficultyLevel, int, int>(DifficultyLevel.Normal, 100, 2),
            new Tuple<DifficultyLevel, int, int>(DifficultyLevel.Normal, 200, 4),
            new Tuple<DifficultyLevel, int, int>(DifficultyLevel.Normal, 300, 8),
            new Tuple<DifficultyLevel, int, int>(DifficultyLevel.Normal, 400, 16),
            new Tuple<DifficultyLevel, int, int>(DifficultyLevel.Normal, 600, 16),

            new Tuple<DifficultyLevel, int, int>(DifficultyLevel.Hard, 100, 2),
            new Tuple<DifficultyLevel, int, int>(DifficultyLevel.Hard, 200, 4),
            new Tuple<DifficultyLevel, int, int>(DifficultyLevel.Hard, 300, 8),
            new Tuple<DifficultyLevel, int, int>(DifficultyLevel.Hard, 400, 16),
            new Tuple<DifficultyLevel, int, int>(DifficultyLevel.Hard, 500, 32),
            new Tuple<DifficultyLevel, int, int>(DifficultyLevel.Hard, 800, 32)
        };

        private static Dictionary<int, List<Tuple<string, int>>> NamesByMaxPoints = new Dictionary<int, List<Tuple<string, int>>>
        {
            { 150, new List<Tuple<string, int>> 
                { 
                    new Tuple<string, int>("Георгий", 1), 
                    new Tuple<string, int>("Федор", 1), 
                    new Tuple<string, int>("Valentin", 1),
                    new Tuple<string, int>("qwerty", 2),
                    new Tuple<string, int>("вирус", 2),
                    new Tuple<string, int>("Олег", 1),
                    new Tuple<string, int>("Тимофей", 3),
                    new Tuple<string, int>("GuitarHero", 1),
                    new Tuple<string, int>("Женя", 2),
                    new Tuple<string, int>("Таня", 1),
                    new Tuple<string, int>("Jim", 3),
                    new Tuple<string, int>("Михаил Иванович", 1),
                    new Tuple<string, int>("доктор-врач", 2),
                    new Tuple<string, int>("Vector", 1)
                } },
            { 350, new List<Tuple<string, int>>
                {
                    new Tuple<string, int>("Тимофей", 2),
                    new Tuple<string, int>("GuitarHero", 1),
                    new Tuple<string, int>("Женя", 3),
                    new Tuple<string, int>("Александр", 2),
                    new Tuple<string, int>("Виктор", 1),
                    new Tuple<string, int>("Эдуард", 1),
                    new Tuple<string, int>("Андрей", 2),
                    new Tuple<string, int>("Sasha", 1),
                    new Tuple<string, int>("Kolian", 3),
                    new Tuple<string, int>("Oops", 2),
                    new Tuple<string, int>("Вадим", 1),
                    new Tuple<string, int>("Михаил Иванович", 1),
                    new Tuple<string, int>("доктор-врач", 2),
                    new Tuple<string, int>("Vector", 1)
            } },
            { 800, new List<Tuple<string, int>> 
                {
                    new Tuple<string, int>("Андрей", 2),
                    new Tuple<string, int>("Sasha", 1),
                    new Tuple<string, int>("Kolian", 3),
                    new Tuple<string, int>("Максим", 1),
                    new Tuple<string, int>("Владислав", 2),
                    new Tuple<string, int>("Сергей", 2),
                    new Tuple<string, int>("Алексей", 1),
                    new Tuple<string, int>("Shpakov", 2),
                    new Tuple<string, int>("Рита", 1),
                    new Tuple<string, int>("Вячеслав", 2),
                    new Tuple<string, int>("Solidny", 1),
                    new Tuple<string, int>("Женя", 3),
                    new Tuple<string, int>("Александр", 2),
                    new Tuple<string, int>("Виктор", 1),
            } }
        };
        
        private static List<int> DiscreteLowPoints = new () { 10, 15, 20, 22, 25, 30, 35, 37, 40, 45, 50, 52, 55, 60, 65, 67, 70, 75, 80, 82, 85, 90, 95, 97, 100 };

        private static readonly Dictionary<DifficultyLevel, List<Tuple<int, int, decimal>>>
            PointsProbabilityConfigurationInitialized = new();

        private static readonly Dictionary<int, List<Tuple<string, int>>> NamesProbabilityConfigurationInitialized = new();

        public static async Task Run(IGameService gameService, INumberService numberService)
        {
            var startDate = new DateTime(2022, 02, 09);
            var endDate = new DateTime(2022, 02, 28);
            
            #region Initialize Points Probabilities
            
            foreach (var difficultyLevel in Enum.GetValues<DifficultyLevel>().Where(x => x != DifficultyLevel.Tutorial))
            {
                var currentMin = 0;
                
                var configs = PointsProbabilityConfiguration.Where(x => x.Item1 == difficultyLevel);

                PointsProbabilityConfigurationInitialized[difficultyLevel] = new List<Tuple<int, int, decimal>>();
                var configToInit = PointsProbabilityConfigurationInitialized[difficultyLevel];

                var currentMinForProbability = 0m;
                foreach (var config in configs)
                {
                    var currentMaxForProbability = currentMinForProbability + decimal.Divide(1, config.Item3);
                    configToInit.Add(new Tuple<int, int, decimal>(currentMin, config.Item2, currentMaxForProbability));

                    currentMin = config.Item2 + 1;
                    currentMinForProbability = currentMaxForProbability;
                }

                if (currentMinForProbability != 1)
                {
                    throw new ArithmeticException("Wrong calculated probability.");
                }
            }

            #endregion

            #region Initialize Names Probabilities

            foreach(var keyValuePair in NamesByMaxPoints)
            {
                NamesProbabilityConfigurationInitialized[keyValuePair.Key] = new List<Tuple<string, int>>();
                var configsToInit = NamesProbabilityConfigurationInitialized[keyValuePair.Key];

                var currentMinForProbability = 0;
                foreach(var config in keyValuePair.Value)
                {
                    var currentMaxForProbability = currentMinForProbability + config.Item2;
                    configsToInit.Add(new Tuple<string, int>(config.Item1, currentMaxForProbability));

                    currentMinForProbability = currentMaxForProbability;
                }
            }

            #endregion

            #region Initialize Numbers

            var numbers = await numberService.GetNumbers(x => x.TutorialLevel == null && x.Length >= 7 && x.Length <= 12, new List<RegularityType>());
            var numbersBylevels = numbers.GroupBy(x =>
                x.Length <= 8 ? DifficultyLevel.Easy :
                x.Length <= 10 ? DifficultyLevel.Normal : DifficultyLevel.Hard);
            var numbersDict = numbersBylevels.ToDictionary(x => x.Key, x => x.ToList());

            #endregion

            var data = new List<Game>();
            var sessionIds = new Dictionary<string, Guid>();

            var currDate = startDate;
            while(currDate <= endDate)
            {
                var daysFromBeginning = (currDate - MinDate).TotalDays.ToInt();
                var dailyNumberOfPlayers = 30 + daysFromBeginning / 3;

                foreach (var difficultyLevel in Enum.GetValues<DifficultyLevel>().Where(x => x != DifficultyLevel.Tutorial))
                {
                    var pointsConfig = PointsProbabilityConfigurationInitialized[difficultyLevel];
                    
                    var dateTimeCollection = new List<DateTime>();

                    #region Generate DateTimes

                    var first3HoursNumberOfPlayers = dailyNumberOfPlayers / 4;
                    var afterFirst3HoursNumberOfPlayers = dailyNumberOfPlayers - first3HoursNumberOfPlayers;

                    var startTime = new TimeSpan(0, 0, 0);
                    var endTime = new TimeSpan(3, 0, 0);
                    var averageInterval = (endTime - startTime) / first3HoursNumberOfPlayers;
                    var maximumAddition = averageInterval / 2;
                    var random = new Random();
                    for (var i = 0; i < first3HoursNumberOfPlayers; i++)
                    {
                        var addMilliseconds = random.Next(maximumAddition.TotalMilliseconds.ToInt());
                        var time = averageInterval * i + TimeSpan.FromMilliseconds(addMilliseconds);
                        dateTimeCollection.Add(new DateTime(currDate.Year, currDate.Month, currDate.Day) + time);
                    }

                    startTime = new TimeSpan(3, 0, 0);
                    endTime = new TimeSpan(24, 0, 0);
                    averageInterval = (endTime - startTime) / afterFirst3HoursNumberOfPlayers;
                    maximumAddition = averageInterval / 2;
                    for (var i = 0; i < afterFirst3HoursNumberOfPlayers; i++)
                    {
                        var addMilliseconds = random.Next(maximumAddition.TotalMilliseconds.ToInt());
                        var time = averageInterval * i + TimeSpan.FromMilliseconds(addMilliseconds);
                        dateTimeCollection.Add(new DateTime(currDate.Year, currDate.Month, currDate.Day) + time);
                    }

                    #endregion

                    var pointsCollection = new List<int>();
                    
                    #region Generate Points
                    
                    var maxLowPoint = DiscreteLowPoints.Max();

                    random = new Random(); 
                    for (var i = 0; i < dailyNumberOfPlayers; i++)
                    {
                        var probabilityValue = (decimal)random.NextDouble();
                        var (minPoints, maxPoints, _) = pointsConfig.First(x => x.Item3 == pointsConfig.Where(y => y.Item3 >= probabilityValue).Min(y => y.Item3));
                        var points = random.Next(minPoints, maxPoints);

                        if (points <= maxLowPoint)
                        {
                            points = DiscreteLowPoints.First(x => Math.Abs(points - x) == DiscreteLowPoints.Min(y => Math.Abs(points - y)));
                        }
                        
                        pointsCollection.Add(points);
                    }
                    
                    #endregion

                    var namePointsDateCollection = new List<Tuple<string, int, DateTime>>();
                    
                    #region Bound Names To Previously Generated Data

                    random = new Random();
                    var j = 0;
                    foreach (var points in pointsCollection)
                    {
                        #region Adjust Points

                        var probabilityValue = random.NextDouble();

                        var currPoints = points;
                        var nameIsEmpty = probabilityValue <= ProbabilityOfNoNamePlayer;

                        if (nameIsEmpty)
                        {
                            var lastDigit = currPoints % 10;
                            if (lastDigit >= 3 && lastDigit <= 7)
                            {
                                currPoints = currPoints - lastDigit + 5;
                            }
                            else if (lastDigit >= 0 && lastDigit <= 2)
                            {
                                currPoints = currPoints - lastDigit;
                            }
                            else
                            {
                                currPoints = currPoints - lastDigit + 10;
                            }
                        }

                        #endregion

                        #region Generate Name

                        string name;
                        if (nameIsEmpty)
                        {
                            name = null;
                        }
                        else
                        {
                            var configKey = NamesProbabilityConfigurationInitialized.Keys.Where(x => x >= currPoints).Min();
                            var configs = NamesProbabilityConfigurationInitialized[configKey];

                            var maxProbability = configs.Max(x => x.Item2);
                            var probabilityValueInt = random.Next(maxProbability);

                            var namesConfig = configs.First(x => x.Item2 == configs.Where(y => y.Item2 >= probabilityValueInt).Min(y => y.Item2));

                            name = namesConfig.Item1;
                        }
                        namePointsDateCollection.Add(new Tuple<string, int, DateTime>(name, currPoints, dateTimeCollection[j]));

                        #endregion

                        j++;
                    }

                    #endregion

                    data.AddRange(namePointsDateCollection.OrderBy(x => x.Item3).Select(x => {
                        var sessionId = x.Item1 == null ? Guid.NewGuid() : sessionIds.ContainsKey(x.Item1) ? sessionIds[x.Item1] : Guid.Empty;
                        if (sessionId == Guid.Empty)
                        {
                            sessionIds[x.Item1] = sessionId = Guid.NewGuid();
                        }

                        var index = random.Next(numbersDict[difficultyLevel].Count - 1);

                        return new Game
                        {
                            VirtualRecord = true,
                            DifficultyLevel = difficultyLevel,
                            NumberId = numbersDict[difficultyLevel][index].Id,
                            SessionId = sessionId.ToString(),
                            StartTime = x.Item3,
                            FinishTime = x.Item1 == null ? null : x.Item3.AddMinutes(4),
                            PlayerName = x.Item1,
                            Score = x.Item2
                        };
                    }));
                }
                
                currDate = currDate.AddDays(1);
            }

            await gameService.AddVirtualGames(data);
        }

        public static int ToInt(this double value)
        {
            return Convert.ToInt32(Math.Floor(value));
        }
    }
}
