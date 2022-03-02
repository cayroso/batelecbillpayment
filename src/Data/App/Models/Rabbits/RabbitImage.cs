using Data.App.Models.FileUploads;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.App.Models.Rabbits
{
    public class RabbitImage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string RabbitImageId { get; set; }

        public string RabbitId { get; set; }
        public virtual Rabbit Rabbit { get; set; }

        public string ImageId { get; set; }
        public virtual FileUpload Image { get; set; }        
    }

    public class RabbitImageConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<RabbitImage>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<RabbitImage> b)
        {
            b.ToTable("RabbitImage");
            b.HasKey(e => e.RabbitImageId);
            b.HasIndex(e => new { e.RabbitId, e.ImageId }).IsUnique();

            b.Property(e => e.RabbitImageId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.RabbitId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.ImageId).HasMaxLength(KeyMaxLength).IsRequired();            
        }
    }
}
