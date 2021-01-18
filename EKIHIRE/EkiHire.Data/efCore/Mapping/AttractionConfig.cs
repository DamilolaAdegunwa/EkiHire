using System;
using System.Collections.Generic;
using System.Text;
using EkiHire.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EkiHire.Data.efCore.Mapping
{
    class AttractionConfig : IEntityTypeConfiguration<Attraction>
    {
        public void Configure(EntityTypeBuilder<Attraction> builder)
        {
            builder.ToTable(nameof(Attraction));
        }
    }
}
