using System.Collections.Generic;
using System.Threading.Tasks;
using NiceNumber.Domain.Entities;

namespace NiceNumber.Services.Interfaces
{
    public interface IRegularityService
    {
        Task<List<Regularity>> GetAllRegularities();
        Task<Regularity> SaveRegularity(Regularity regularity);
        Task SaveChanges();
        Task RemoveRegularitiesMarkedAsDeleted();
    }
}