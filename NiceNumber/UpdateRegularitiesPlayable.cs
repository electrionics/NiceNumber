using System;
using System.Threading.Tasks;
using NiceNumber.Core;
using NiceNumber.Domain.Entities;
using NiceNumber.Services.Interfaces;

namespace NiceNumber
{
    public static class UpdateRegularitiesPlayable
    {
        public static async Task Run(IRegularityService regularityService)
        {
            var regularities = await regularityService.GetAllRegularities();

            foreach (var regularity in regularities)
            {
                regularity.Playable = EvaluatePlayable(regularity);
            }

            await regularityService.SaveChanges();
        }

        public static bool EvaluatePlayable(Regularity regularity)
        {
            return Math.Abs(regularity.RegularityNumber) <= 100 &&
                   (regularity.Type != RegularityType.GeometricProgression || regularity.RegularityNumber >= 0.01);
        }
    }
}