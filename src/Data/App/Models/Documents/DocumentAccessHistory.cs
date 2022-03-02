using Cayent.Core.Common.Extensions;
using Data.App.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.App.Models.Documents
{
    public class DocumentAccessHistory
    {
        public string DocumentAccessHistoryId { get; set; }

        public string DocumentId { get; set; }
        public virtual Document Document { get; set; }

        public string AccessedById { get; set; }
        public virtual User AccessedBy { get; set; }

        DateTime _dateAccessed;
        public DateTime DateAccessed
        {
            get => _dateAccessed;
            set => _dateAccessed = value.Truncate().AsUtc();
        }
    }
}
