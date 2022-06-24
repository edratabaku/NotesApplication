using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Noteapp.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noteapp.Data.Mapping.Note
{
    class NoteMap : BaseEntityTypeConfiguration<Noteapp.Domain.Models.Note.Note>
    {
        public override int Order => 1;
        public override void Configure(EntityTypeBuilder<Domain.Models.Note.Note> builder)
        {
            builder.MapDefaults();
            builder.Property(mapping => mapping.Title).IsRequired().HasMaxLength(100);
            builder.Property(mapping => mapping.Description).IsRequired().HasMaxLength(2000);
            builder.Property(mapping => mapping.Tags).IsRequired().HasMaxLength(1000);
            builder.MapAuditableEntity();
            base.Configure(builder);
        }
        protected override void PostConfigure(EntityTypeBuilder<Domain.Models.Note.Note> builder)
        {
            base.PostConfigure(builder);
        }
    }
}
