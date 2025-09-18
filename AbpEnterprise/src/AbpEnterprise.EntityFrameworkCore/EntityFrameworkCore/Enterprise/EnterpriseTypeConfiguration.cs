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
    public class EnterpriseTypeConfiguration : IEntityTypeConfiguration<EnterpriseType>
    {
        public void Configure(EntityTypeBuilder<EnterpriseType> b)
        {
            b.ToTable(AbpEnterpriseConsts.DbTablePrefix + "EnterpriseTypes");
            b.ConfigureByConvention();

            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(EnterpriseTypeConsts.MaxNameLength);

            b.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(EnterpriseTypeConsts.MaxCodeLength);

            b.Property(x => x.Description)
                .HasMaxLength(EnterpriseTypeConsts.MaxDescriptionLength);

            b.HasIndex(x => new { x.EnterpriseIndustryId, x.Code }).IsUnique();
            b.HasIndex(x => x.Name);
            b.HasIndex(x => x.IsActive);

            b.HasMany(x => x.AddressMappings)
                .WithOne()
                .HasForeignKey(x => x.EnterpriseTypeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
