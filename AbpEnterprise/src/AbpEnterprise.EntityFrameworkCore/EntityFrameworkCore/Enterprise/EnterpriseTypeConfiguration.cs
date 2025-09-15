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
    public class EnterpriseTypeConfiguration : IEntityTypeConfiguration<EnterpriseType>
    {
        public void Configure(EntityTypeBuilder<EnterpriseType> builder)
        {
            builder.ToTable("EnterpriseTypes");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Description)
                .HasMaxLength(500);

            // Cấu hình quan hệ 1-n
            builder.HasMany(e => e.Industries)
                .WithOne()
                .HasForeignKey("EnterpriseTypeId")
                .OnDelete(DeleteBehavior.Cascade); // Xóa EnterpriseType sẽ xóa tất cả EnterpriseIndustry liên quan
        }
    }
}
