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
        public void Configure(EntityTypeBuilder<EnterpriseType> builder)
        {
            builder.ToTable(AbpEnterpriseConsts.DbTablePrefix + "EnterpriseTypes");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Description)
                .HasMaxLength(500);

            builder.HasMany(e => e.Industries)
                .WithOne()
                .HasForeignKey("EnterpriseTypeId")
                .OnDelete(DeleteBehavior.Cascade);
            builder.ConfigureByConvention();
        }
    }
}
