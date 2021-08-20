using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NiceNumber.Core.Regularities;
using NiceNumber.Domain.Entities;
using NiceNumber.Services.Interfaces;

namespace NiceNumber
{
    public static class UpdateRegularities
    {
        public static async Task Run(INumberService numberService, IRegularityService regularityService)
        {
            var regularities = new List<IRegularity>
            {
                new MirrorDigits()
            };

            var typesToUpdate = regularities.SelectMany(x => x.PossibleTypes).ToList();
            
            var numberLength = 12;

            var numbers = await numberService.GetNumbers(numberLength, typesToUpdate);

            foreach (var number in numbers)
            {
                foreach (var regularity in regularities)
                {
                    var result = regularity.Process(number.Value);

                    var existingRegularities = number.Regularities.Where(x =>
                            regularity.PossibleTypes.Contains(x.Type))
                        .ToHashSet(Regularity.RegularityComparer);
                    var newRegularities = result.Select(reg => new Regularity
                    {
                        NumberId = number.Id,
                        RegularityNumber = reg.RegularityNumber,
                        SequenceType = reg.SequenceType,
                        Type = reg.Type,
                        StartPositionsStr = string.Join(',', reg.Positions),
                        SubNumberLengthsStr = string.Join(',', reg.SubNumberLengths)
                    }).ToHashSet(Regularity.RegularityComparer);

                    var regularitiesToDelete = existingRegularities
                        .Where(x => !newRegularities.Contains(x))
                        .ToList();
                    var regularitiesToAdd = newRegularities
                        .Where(x => !existingRegularities.Contains(x))
                        .ToList();

                    foreach (var entity in regularitiesToDelete)
                    {
                        entity.Deleted = true;
                    }
                    
                    await regularityService.SaveChanges();
                    
                    foreach (var entity in regularitiesToAdd)
                    {
                        await regularityService.SaveRegularity(entity);
                    }
                }
            }

            await regularityService.RemoveRegularitiesMarkedAsDeleted();
        }
    }
}