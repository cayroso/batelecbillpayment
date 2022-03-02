using App.CQRS.Documents.Common.Queries.Query;
using Cayent.Core.CQRS.Queries;
using Data.App.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.CQRS.Documents.Common.Queries.Handler
{
    public sealed class DocumentCommonQueryHandler :
        IQueryHandler<GetDocumentByIdQuery, GetDocumentByIdQuery.Document>
    {
        readonly AppDbContext _dbContext;
        public DocumentCommonQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        async Task<GetDocumentByIdQuery.Document> IQueryHandler<GetDocumentByIdQuery, GetDocumentByIdQuery.Document>.HandleAsync(GetDocumentByIdQuery query, System.Threading.CancellationToken cancellationToken)
        {
            var sql = from doc in _dbContext.Documents.AsNoTracking()
                                    .Include(e => e.FileUpload)
                                    .Include(e => e.UploadedBy)
                                    .Include(e => e.DocumentAccessHistories)
                                        .ThenInclude(e => e.AccessedBy)

                      where doc.DocumentId == query.DocumentId

                      select new GetDocumentByIdQuery.Document
                      {
                          DocumentId = doc.DocumentId,
                          Name = doc.Name,
                          Description = doc.Description,

                          DateCreated = doc.DateCreated,
                          DateUpdated = doc.DateUpdated,

                          UploadedBy = doc.UploadedBy.FirstLastName,

                          FileUpload = new GetDocumentByIdQuery.FileUpload
                          {
                              Url = doc.FileUpload.Url,
                              ContentType = doc.FileUpload.ContentType,
                              FileName = doc.FileUpload.FileName,
                              Length = doc.FileUpload.Length,
                          },

                          Token = doc.ConcurrencyToken,

                          AccessHistories = doc.DocumentAccessHistories.Select(e => new GetDocumentByIdQuery.DocumentAccessHistory
                          {
                              AccessedBy = e.AccessedBy.FirstLastName,
                              DateAccessed = e.DateAccessed
                          })

                      };

            var dto = await sql.FirstOrDefaultAsync();

            return dto;
        }
    }
}
