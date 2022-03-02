using Data.App.Models.Cages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.App.Models.LitterBatches
{
    public class LitterBatch
    {
        public string CageId { get; set; }
        public virtual Cage Cage { get; set; }

        public virtual ICollection<LitterBatchBreeding> LitterBatchBreedings { get; set; } = new List<LitterBatchBreeding>();
    }

    public class LitterBatchBreeding
    {
        public string LitterBatchId { get; set; }
        public string BreedingId { get; set; }

        public int Bucks { get; set; }
        public int Does { get; set; }
        public int Total { get; set; }

    }
}
