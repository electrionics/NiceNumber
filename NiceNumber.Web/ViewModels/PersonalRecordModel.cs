using NiceNumber.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NiceNumber.Web.ViewModels
{
    public class PersonalRecordModel
    {
        public string Player { get; set; }

        public int Score { get; set; }

        public DifficultyLevel DifficultyLevel { get; set; }
    }
}
