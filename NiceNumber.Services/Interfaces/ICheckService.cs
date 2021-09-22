using System;
using System.Threading.Tasks;
using NiceNumber.Core;
using NiceNumber.Domain.Entities;

namespace NiceNumber.Services.Interfaces
{
    public interface ICheckService
    {
        Task<CheckResult> CheckRegularity(Guid gameId, string sessionId, byte[] positions, RegularityType type, bool hinted);
        Task<HintResult> GetRandomCheck(Guid gameId, string sessionId, RegularityType? type, double? regularityNumber);
    }
}