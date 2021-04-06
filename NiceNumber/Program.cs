using System;
using System.Collections.Generic;
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
            
            var regularities = new List<IRegularity>
            {
                new SameDigitsSequential(),
                new SameDigitsWithFixedGap(),
                //new SameDigitsAtAnyPosition(),
                new ArithmeticProgressionSequential()
            };

            for (var i = minNumber; i < maxNumber; i++)
            {
                result[i] = new List<RegularityDetectResult>();
                
                foreach (var regularity in regularities)
                {
                    result[i].AddRange(regularity.Process(i));
                }
            }

            var report1 = result.GroupBy(x => x.Value.Count);
            var report2 = result.SelectMany(x => x.Value).GroupBy(x => x.Type);

            foreach (var repItem in report1)
            {
                Console.WriteLine($"{repItem.Key} - количество закономерностей в одном номере, всего номеров: {repItem.Count()}");
            }
            
            foreach (var repItem in report2)
            {
                Console.WriteLine($"{repItem.Key} - закономерность данного типа, всего номеров: {repItem.Count()}");
            }

            Console.ReadKey();
        }
    }
}