using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NiceNumber.Core;
using NiceNumber.Core.Regularities;
using NiceNumber.Core.Regularities.Deprecated;
using NiceNumber.Core.Results;
using NiceNumber.Domain;
using NiceNumber.Services.Implementation;
using NiceNumber.Services.Interfaces;

namespace NiceNumber
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddScoped<INumberService, NumberService>()
                .AddScoped<IRegularityService, RegularityService>()
                .AddDbContext<NumberDataContext>(builder => 
                    builder.UseSqlServer("server=.;database=numio;trusted_connection=true;"))//server=93.125.99.108;database=nummiesr_numio;User Id=nummiesr_prod;Password=aquila899FU3P!!!;
                .BuildServiceProvider();

            var numberService = serviceProvider.GetService<INumberService>();
            var regularityService = serviceProvider.GetService<IRegularityService>();

            //await AddNumbers.Run(numberService, regularityService);
            //await UpdateRegularities.Run(numberService, regularityService);
            await UpdateRegularitiesPlayable.Run(regularityService);

            //Test4();
            //Test3();
            //Test2();
            //Test1();
        }

        static void Test4()
        {
            //var number = ;
            var regularities = new List<IRegularity>
            {
                new GeometricProgression(),
                new ArithmeticProgression(),
                new MirrorDigits(),
                new Multiples(),
                new SameNumbers()
            };
            
            Console.WriteLine("Hello");

            var random = new Random();
            var numbersDigits = Enumerable.Range(1, 10).Select(x =>
                Enumerable.Range(1, 10)
                    .Select(y => random.Next(0, 9))
                    .Prepend(random.Next(1, 9))
                    .ToArray()).ToArray();

            var numbers = new List<long>();
            foreach (var numberDigits in numbersDigits)
            {
                var number = (long) numberDigits[0];
                
                foreach (var digit in numberDigits) number = number * 10 + digit;

                numbers.Add(number);
            }

            //numbers = new List<long>(){ 1024595542219 }; 
            
            foreach (var number in numbers)
            {
                Console.WriteLine(number);

                var totalCount = 0;
                
                foreach (var regularity in regularities)
                {
                    var sw = new Stopwatch();
                    sw.Start();

                    var result = regularity.Process(number);
                
                    sw.Stop();
                    var time = sw.ElapsedMilliseconds;
                    var seconds = time / (double)1000;

                    Console.WriteLine($"'{regularity.GetType().Name}' Detected: {result.Count}. Time: {seconds} s.");

                    totalCount += result.Count;
                }
                
                Console.WriteLine($"Total regularities count: {totalCount}.");
                Console.WriteLine();
            }

            Console.ReadKey();
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
                time[regularity.MainType] = sw.ElapsedMilliseconds;
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