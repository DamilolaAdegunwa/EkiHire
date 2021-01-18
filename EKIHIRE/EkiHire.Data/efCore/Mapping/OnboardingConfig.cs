using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EkiHire.Core.Domain.Entities;
namespace EkiHire.Data.efCore.Mapping
{
    public class OnboardingConfig : IEntityTypeConfiguration<Onboarding>
    {
        public void Configure(EntityTypeBuilder<Onboarding> builder)
        {
            builder.ToTable(nameof(Onboarding));
        }
    }
}
