using Cayent.Core.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.App.Models.FileUploads
{
    public class FileUpload
    {
        public string FileUploadId { get; set; }
        public string Url { get; set; }
        public string FileName { get; set; }
        public string ContentDisposition { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public long Length { get; set; }

        DateTime _dateCreated = DateTime.Now.Truncate();
        public DateTime DateCreated
        {
            get => _dateCreated.AsUtc();
            set => _dateCreated = value.Truncate();
        }
    }
}
