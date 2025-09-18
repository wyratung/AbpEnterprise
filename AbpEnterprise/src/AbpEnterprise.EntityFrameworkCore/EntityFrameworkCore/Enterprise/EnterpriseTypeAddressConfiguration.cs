using AbpEnterprise.Enterpries.Enum;
using AbpEnterprise.Enterprises;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace AbpEnterprise.EntityFrameworkCore.Enterprise;

public class EnterpriseTypeAddressConfiguration : IEntityTypeConfiguration<EnterpriseTypeAddress>
{
    public void Configure(EntityTypeBuilder<EnterpriseTypeAddress> b)
    {
        b.ToTable(AbpEnterpriseConsts.DbTablePrefix + "EnterpriseTypeAddresses");
        
        b.ConfigureByConvention();

        b.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(EnterpriseTypeAddressConsts.MaxNameLength);

        b.Property(x => x.Street)
            .IsRequired()
            .HasMaxLength(EnterpriseTypeAddressConsts.MaxStreetLength);

        b.Property(x => x.City)
            .IsRequired()
            .HasMaxLength(EnterpriseTypeAddressConsts.MaxCityLength);

        b.Property(x => x.State)
            .HasMaxLength(EnterpriseTypeAddressConsts.MaxStateLength);

        b.Property(x => x.Country)
            .IsRequired()
            .HasMaxLength(EnterpriseTypeAddressConsts.MaxCountryLength);

        b.Property(x => x.ZipCode)
            .HasMaxLength(EnterpriseTypeAddressConsts.MaxZipCodeLength);

        b.Property(x => x.ContactPhone)
            .HasMaxLength(EnterpriseTypeAddressConsts.MaxContactPhoneLength);

        b.Property(x => x.ContactEmail)
            .HasMaxLength(EnterpriseTypeAddressConsts.MaxContactEmailLength);

        b.Property(x => x.AddressType)
            .IsRequired().HasDefaultValue(AddressType.Office);
        // Unique constraint for location
        b.HasIndex(x => new { x.Street, x.City, x.State, x.Country, x.ZipCode })
            .HasDatabaseName("IX_EnterpriseTypeAddresses_Location");

        b.HasMany(x => x.TypeMappings)
            .WithOne()
            .HasForeignKey(x => x.EnterpriseTypeAddressId)
            .OnDelete(DeleteBehavior.Cascade);  
    }
}