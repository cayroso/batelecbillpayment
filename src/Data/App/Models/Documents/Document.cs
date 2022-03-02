using Cayent.Core.Common.Extensions;
using Data.App.Models.FileUploads;
using Data.App.Models.Users;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.App.Models.Documents
{
    public class Document
    {
        public string DocumentId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        //public string FileUploadId { get; set; }
        public virtual FileUpload FileUpload { get; set; }

        public string UploadedById { get; set; }
        public virtual User UploadedBy { get; set; }


        DateTime _dateCreated;
        public DateTime DateCreated
        {
            get => _dateCreated;
            set => _dateCreated = value.Truncate().AsUtc();
        }

        DateTime _dateUpdated;
        public DateTime DateUpdated
        {
            get => _dateUpdated;
            set => _dateUpdated = value.Truncate().AsUtc();
        }

        public string ConcurrencyToken { get; set; } = Guid.NewGuid().ToString();

        public ICollection<DocumentAccessHistory> DocumentAccessHistories { get; set; } = new List<DocumentAccessHistory>();
    }

    public static class DocumentExtension
    {
        public static void ThrowIfNull(this Document me)
        {
            if (me == null)
                throw new ApplicationException("Not found.");
        }
        public static void ThrowIfNullOrAlreadyUpdated(this Document me, string currentToken, string newToken)
        {
            me.ThrowIfNull();

            if (string.IsNullOrWhiteSpace(newToken))
                throw new ApplicationException("Anti-forgery token not found.");

            if (me.ConcurrencyToken != currentToken)
                throw new ApplicationException("Already updated by another user.");

            me.ConcurrencyToken = newToken;
        }
    }
}
