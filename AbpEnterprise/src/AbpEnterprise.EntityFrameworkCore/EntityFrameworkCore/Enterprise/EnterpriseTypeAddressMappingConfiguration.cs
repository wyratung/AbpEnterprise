using AbpEnterprise.Enterprises;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace AbpEnterprise.EntityFrameworkCore.Enterprise
{
    public class EnterpriseTypeAddressMappingConfiguration : IEntityTypeConfiguration<EnterpriseTypeAddressMapping>
    {
        public void Configure(EntityTypeBuilder<EnterpriseTypeAddressMapping> b)
        {
            b.ToTable(AbpEnterpriseConsts.DbTablePrefix + "EnterpriseTypeAddressMappings");

            
            // Unique constraint for enterprise type and address combination
            b.HasIndex(x => new { x.EnterpriseTypeId, x.EnterpriseTypeAddressId })
                .IsUnique()
                .HasDatabaseName("IX_EnterpriseTypeAddressMappings_Unique");

            b.HasIndex(x => x.EnterpriseTypeId);
            b.HasIndex(x => x.EnterpriseTypeAddressId);
            

            // Navigation properties (configured as optional for flexibility)
            // Since this is a junction between two aggregate roots, 
            // we avoid direct navigation to maintain aggregate boundaries
            b.Ignore(x => x.EnterpriseType);
            b.Ignore(x => x.EnterpriseTypeAddress);


        }
    }
}