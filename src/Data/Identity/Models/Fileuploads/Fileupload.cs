using Cayent.Core.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Identity.Models.Fileuploads
{
    public class Fileupload
    {
        public string FileuploadId { get; set; }
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

    internal class FileuploadConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<Fileupload>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Fileupload> b)
        {
            b.ToTable("Fileupload");
            b.HasKey(e => e.FileuploadId);

            b.Property(e => e.FileuploadId).HasMaxLength(KeyMaxLength).IsRequired();
        }
    }
}
