using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NiceNumber.Domain;
using NiceNumber.Domain.Entities;
using NiceNumber.Services.Interfaces;

namespace NiceNumber.Services.Implementation
{
    public class TutorialService: ITutorialService
    {
        private readonly NumberDataContext _dataContext;

        public TutorialService(NumberDataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
        public async Task<Tuple<int, int>> GetTutorialCounts()
        {
            var levelsCount = await _dataContext.Set<TutorialLevel>()
                .CountAsync();
            var tasksCount = await _dataContext.Set<TutorialTask>()
                .CountAsync();

            return new Tuple<int, int>(levelsCount, tasksCount);
        }
    }
}