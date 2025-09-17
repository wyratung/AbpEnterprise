using AbpEnterprise.Enterprises;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace AbpEnterprise.EntityFrameworkCore.Enterprise
{
    public class EnterpriseIndustryConfiguration : IEntityTypeConfiguration<EnterpriseIndustry>
    {
        public void Configure(EntityTypeBuilder<EnterpriseIndustry> b)
        {
            b.ToTable(AbpEnterpriseConsts.DbTablePrefix + "EnterpriseIndustries");
            b.ConfigureByConvention();

            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(EnterpriseIndustryConsts.MaxNameLength);

            b.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(EnterpriseIndustryConsts.MaxCodeLength);

            b.Property(x => x.Description)
                .HasMaxLength(EnterpriseIndustryConsts.MaxDescriptionLength);

            b.HasIndex(x => x.Code).IsUnique();
            b.HasIndex(x => x.Name);
            b.HasIndex(x => x.IsActive);

            // Configure relationship
            b.HasMany(x => x.EnterpriseTypes)
                .WithOne()
                .HasForeignKey(x => x.EnterpriseIndustryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
