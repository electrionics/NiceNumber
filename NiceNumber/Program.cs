using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NiceNumber.Regularities;
using NiceNumber.Results;

namespace NiceNumber
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var minNumber = 1000000;
            var maxNumber = 9999999;
            var result = new Dictionary<int, List<RegularityDetectResult>>();
            var time = new Dictionary<RegularityType, long>();
            
            var regularities = new List<IRegularity>
            {
                // new SameDigitsSequential(),
                // new SameDigitsWithFixedGap(),
                // new SameDigitsAtAnyPosition(),
                // new ArithmeticProgressionSequential(),
                // new ArithmeticProgressionWithFixedGap(),
                new ArithmeticProgressionAtAnyPosition()
            };

            foreach (var regularity in regularities)
            {
                var sw = new Stopwatch();
                sw.Start();
                
                for (var i = minNumber; i < maxNumber; i++)
                {
                    if (!result.ContainsKey(i))
                    {
                        result[i] = new List<RegularityDetectResult>();
                    }
                    
                    result[i].AddRange(regularity.Process(i));
                }
                
                sw.Stop();
                time[regularity.Type] = sw.ElapsedMilliseconds;
            }

            var report1 = result.GroupBy(x => x.Value.Count);
            var report2 = result.SelectMany(x => x.Value).GroupBy(x => x.Type);

            foreach (var repItem in report1)
            {
                Console.WriteLine($"{repItem.Key} - count of regularities in one number, total numbers: {repItem.Count()}");
            }
            
            foreach (var repItem in report2)
            {
                Console.WriteLine($"{repItem.Key} - regularity of given type, total numbers: {repItem.Count()}");
            }

            foreach (var item in time)
            {
                Console.WriteLine($"Type: {item.Key} Time: {item.Value / .1000} s");
            }

            Console.ReadKey();
        }
    }
}