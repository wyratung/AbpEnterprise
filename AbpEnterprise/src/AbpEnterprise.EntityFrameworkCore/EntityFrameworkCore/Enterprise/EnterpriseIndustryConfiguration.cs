using AbpEnterprise.Enterprises;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpEnterprise.EntityFrameworkCore.Enterprise
{
    public class EnterpriseIndustryConfiguration : IEntityTypeConfiguration<EnterpriseIndustry>
    {
        public void Configure(EntityTypeBuilder<EnterpriseIndustry> builder)
        {
            builder.ToTable("EnterpriseIndustries");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.IndustryName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.IndustryCode)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
