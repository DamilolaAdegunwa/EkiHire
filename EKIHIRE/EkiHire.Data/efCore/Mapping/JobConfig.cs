using System;
using System.Collections.Generic;
using System.Text;
using EkiHire.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace EkiHire.Data.efCore.Mapping
{
    class JobConfig : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.ToTable(nameof(Job));
        }
    }
}
