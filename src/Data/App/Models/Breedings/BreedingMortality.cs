using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cayent.Core.Common.Extensions;

namespace Data.App.Models.Breedings
{
    public class BreedingMortality
    {
        public string BreedingMortalityId { get; set; }

        public string BreedingId { get; set; }
        public virtual Breeding Breeding { get; set; }

        public EnumMortalityType MortalityType { get; set; }

        public int Count { get; set; }

        public string Notes { get; set; }

        DateTime _dateRecorded;
        public DateTime DateRecorded
        {
            get => _dateRecorded.AsUtc();
            set => _dateRecorded = value.Truncate();
        }
    }
}
