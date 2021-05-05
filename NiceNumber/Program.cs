using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NiceNumber.Core.Regularities;
using NiceNumber.Core.Regularities.Deprecated;
using NiceNumber.Regularities;
using NiceNumber.Results;

namespace NiceNumber
{
    class Program
    {
        static void Main(string[] args)
        {
            Test3();
            //Test2();
            //Test1();
        }
        
        static void Test3()
        {
            Console.WriteLine("Hello");
            
            var number = 1024595542219;//11
            var regularity = new MirrorDigits();
            
            var sw = new Stopwatch();
            sw.Start();

            var result = regularity.Process(number);
            
            sw.Stop();
            var time = sw.ElapsedMilliseconds;
            var seconds = time / (double)1000;
            
            Console.WriteLine($"Time: {seconds} s");
            Console.ReadKey();
        }

        static void Test2()
        {
            Console.WriteLine("Hello");
            
            var number = 8972345798034;//11
            var regularity = new ArithmeticProgression();
            
            var sw = new Stopwatch();
            sw.Start();

            var result = regularity.Process(number);
            
            sw.Stop();
            var time = sw.ElapsedMilliseconds;
            var seconds = time / (double)1000;
            
            Console.WriteLine($"Time: {seconds} s");
            Console.ReadKey();
        }
        
        static void Test1()
        {
            Console.WriteLine("Hello World!");
            var minNumber = 1000000;
            var maxNumber = 9999999;
            var result = new Dictionary<int, List<RegularityDetectResult>>();
            var time = new Dictionary<RegularityType, long>();
            
            var regularities = new List<IRegularity>
            {
                new SameDigitsSequential(),
                new SameDigitsWithFixedGap(),
                new SameDigitsAtAnyPosition(),
                new ArithmeticProgressionSequential(),
                new ArithmeticProgressionWithFixedGap(),
                new ArithmeticProgression()
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