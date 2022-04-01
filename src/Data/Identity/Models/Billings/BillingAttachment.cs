using Data.Identity.Models.Fileuploads;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Identity.Models.Billings
{
    public class BillingAttachment
    {
        public string BillingId { get; set; }
        public virtual Billing Billing { get; set; }
        public string AttachmentId { get; set; }
        public virtual Fileupload Fileupload { get; set; }
    }

    internal class BillingAttachmentConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<BillingAttachment>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<BillingAttachment> b)
        {
            b.ToTable("BillingAttachment");
            b.HasKey(e => new { e.BillingId, e.AttachmentId });

            b.Property(e => e.BillingId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.AttachmentId).HasMaxLength(KeyMaxLength).IsRequired();            
        }
    }
}
