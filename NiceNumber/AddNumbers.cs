using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NiceNumber.Core.Regularities;
using NiceNumber.Domain.Entities;
using NiceNumber.Services.Interfaces;

namespace NiceNumber
{
    public class AddNumbers
    {
        public static async Task Run(INumberService numberService, IRegularityService regularityService)
        {
            var regularities = new List<IRegularity>
            {
                new GeometricProgression(),
                new ArithmeticProgression(),
                new MirrorDigits(),
                new Multiples(),
                new SameNumbers()
            };

            var numberLength = 12;
            
            var random = new Random();
            var numbersDigits = Enumerable.Range(1, 1000).Select(x =>
                Enumerable.Range(1, numberLength - 1)
                    .Select(y => random.Next(0, 9))
                    .Prepend(random.Next(1, 9))
                    .ToArray()).ToArray();

            var numbers = new List<long>();
            foreach (var numberDigits in numbersDigits)
            {
                var number = (long) 0;
                
                foreach (var digit in numberDigits) number = number * 10 + digit;

                numbers.Add(number);
            }
            
            foreach (var number in numbers)
            {
                var numberEntity = new Number
                {
                    Value = number,
                    Length = numberLength
                };

                try
                {
                    await numberService.SaveNumber(numberEntity);
                }
                catch (Exception e)
                {
                    continue;
                }
                
                foreach (var regularity in regularities)
                {
                    var result = regularity.Process(number);
                    var regularityEntities = result.Select(reg => new Regularity
                    {
                        NumberId = numberEntity.Id,
                        RegularityNumber = reg.RegularityNumber,
                        SequenceType = reg.SequenceType,
                        Type = reg.Type,
                        StartPositionsStr = string.Join(',', reg.Positions),
                        SubNumberLengthsStr = string.Join(',', reg.SubNumberLengths)
                    }).ToList();
                    
                    foreach (var entity in regularityEntities)
                    {
                        await regularityService.SaveRegularity(entity);
                    }
                }
            }
        }
    }
}