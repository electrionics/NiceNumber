using System;
using System.Threading.Tasks;
using NiceNumber.Core;
using NiceNumber.Domain.Entities;

namespace NiceNumber.Services.Interfaces
{
    public interface ICheckService
    {
        Task<CheckResult> CheckRegularity(Guid gameId, string sessionId, byte[] positions, RegularityType type);
    }
}