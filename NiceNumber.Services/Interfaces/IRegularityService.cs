using System.Threading.Tasks;
using NiceNumber.Domain.Entities;

namespace NiceNumber.Services.Interfaces
{
    public interface IRegularityService
    {
        Task<Regularity> SaveRegularity(Regularity regularity);
    }
}