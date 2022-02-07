using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceNumber
{
    public static class AddRecords
    {
        public static DateTime MinDate = new DateTime(2022, 02, 07);
        public static async Task Run()
        {
            var minDate = MinDate;
            var probabilityOfNoNamePlayer = 0.6;
            

            var startDate = new DateTime(2022, 02, 07);
            var endDate = new DateTime(2022, 02, 28);

            var currDate = startDate;
            while(currDate <= endDate)
            {
                var dateTimeCollection = new List<DateTime>();

                #region Generate DateTimes

                var daysFromBeginning = (currDate - minDate).TotalDays.ToInt();
                var dailyNumberOfPlayers = 30 + daysFromBeginning / 3;
                var first3hoursNumberOfPlayers = dailyNumberOfPlayers / 4;
                var afterFirst3hoursNumberOfPlayers = dailyNumberOfPlayers - first3hoursNumberOfPlayers;

                var startTime = new TimeSpan(0, 0, 0);
                var endTime = new TimeSpan(3, 0, 0);
                var averageInterval = (endTime - startTime) / first3hoursNumberOfPlayers;
                var maximumAddition = averageInterval / 2;
                var random = new Random();
                for (var i = 0; i < first3hoursNumberOfPlayers; i++)
                {
                    var time = averageInterval * i + new TimeSpan(10 * random.Next(maximumAddition.TotalMilliseconds.ToInt()));
                    dateTimeCollection.Add(new DateTime(currDate.Year, currDate.Month, currDate.Day) + time);
                }

                startTime = new TimeSpan(3, 0, 0);
                endTime = new TimeSpan(24, 0, 0);
                averageInterval = (endTime - startTime) / afterFirst3hoursNumberOfPlayers;
                maximumAddition = averageInterval / 2;
                for (var i = 0; i < afterFirst3hoursNumberOfPlayers; i++)
                {
                    var time = averageInterval * i + new TimeSpan(10 * random.Next(maximumAddition.TotalMilliseconds.ToInt()));
                    dateTimeCollection.Add(new DateTime(currDate.Year, currDate.Month, currDate.Day) + time);
                }

                #endregion

                #region Generate Points

                #endregion

                #region Bound Names To Previously Generated Data

                #endregion

                currDate = currDate.AddDays(1);
            }
        }

        public static int ToInt(this double value)
        {
            return Convert.ToInt32(Math.Floor(value));
        }
    }
}
