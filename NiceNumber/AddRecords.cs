using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NiceNumber.Domain.Entities;

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

        private static Dictionary<int, List<string>> NamesByMaxPoints = new Dictionary<int, List<string>>
        {
            { 150, new List<string> {} },
            { 350, new List<string> {} },
            { 800, new List<string> {} }
        };
        
        private static List<int> DiscreteLowPoints = new () { 10, 15, 20, 22, 25, 30, 35, 37, 40, 45, 50, 52, 55, 60, 65, 67, 70, 75, 80, 82, 85, 90, 95, 97, 100 };

        private static Dictionary<DifficultyLevel, List<Tuple<int, int, decimal>>>
            PointsProbabilityConfigurationInitialized;

        public static async Task Run()
        {
            var startDate = new DateTime(2022, 02, 07);
            var endDate = new DateTime(2022, 02, 28);
            
            #region Initialize Points Probabilities
            
            foreach (var difficultyLevel in Enum.GetValues<DifficultyLevel>())
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

            var currDate = startDate;
            while(currDate <= endDate)
            {
                var daysFromBeginning = (currDate - MinDate).TotalDays.ToInt();
                var dailyNumberOfPlayers = 30 + daysFromBeginning / 3;

                foreach (var difficultyLevel in Enum.GetValues<DifficultyLevel>())
                {
                    var config = PointsProbabilityConfigurationInitialized[difficultyLevel];
                    
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
                        var time = averageInterval * i + new TimeSpan(10 * random.Next(maximumAddition.TotalMilliseconds.ToInt()));
                        dateTimeCollection.Add(new DateTime(currDate.Year, currDate.Month, currDate.Day) + time);
                    }

                    startTime = new TimeSpan(3, 0, 0);
                    endTime = new TimeSpan(24, 0, 0);
                    averageInterval = (endTime - startTime) / afterFirst3HoursNumberOfPlayers;
                    maximumAddition = averageInterval / 2;
                    for (var i = 0; i < afterFirst3HoursNumberOfPlayers; i++)
                    {
                        var time = averageInterval * i + new TimeSpan(10 * random.Next(maximumAddition.TotalMilliseconds.ToInt()));
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
                        var (minPoints, maxPoints, _) = config.First(x => x.Item3 == config.Where(y => y.Item3 >= probabilityValue).Min(y => y.Item3));
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
                    foreach (var points in pointsCollection)
                    {
                        var currPoints = points;
                        
                        var probabilityValue = random.NextDouble();
                        if (probabilityValue <= ProbabilityOfNoNamePlayer)
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
                    }
                    
                    #endregion

                    
                }
                currDate = currDate.AddDays(1);
            }
        }

        public static int ToInt(this double value)
        {
            return Convert.ToInt32(Math.Floor(value));
        }
    }
}
