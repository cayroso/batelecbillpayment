using Cayent.Core.CQRS.Queries;
using Cayent.Core.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.CQRS.Documents.Common.Queries.Query
{
    public sealed class GetDocumentByIdQuery : AbstractQuery<GetDocumentByIdQuery.Document>
    {
        public string DocumentId { get; }

        public GetDocumentByIdQuery(string correlationId, string tenantId, string userId, string documentId)
            : base(correlationId, tenantId, userId)
        {
            DocumentId = documentId;
        }

        public class Document
        {
            public string DocumentId { get; set; }

            public string Name { get; set; }
            public string Description { get; set; }
            
            public virtual FileUpload FileUpload { get; set; }

            public string UploadedBy { get; set; }


            DateTime _dateCreated;
            public DateTime DateCreated
            {
                get => _dateCreated;
                set => _dateCreated = value.AsUtc();
            }

            DateTime _dateUpdated;
            public DateTime DateUpdated
            {
                get => _dateUpdated;
                set => _dateUpdated = value.AsUtc();
            }

            public string Token { get; set; }

            public IEnumerable<DocumentAccessHistory> AccessHistories { get; set; }
        }

        public class FileUpload
        {            
            public string Url { get; set; }
            public string FileName { get; set; }            
            public string ContentType { get; set; }            
            public long Length { get; set; }
        }

        public class DocumentAccessHistory
        {
            public string AccessedBy { get; set; }

            DateTime _dateAccessed;
            public DateTime DateAccessed
            {
                get => _dateAccessed;
                set => _dateAccessed = value.AsUtc();
            }
        }
    }
}
