using AbpEnterprise.Enterpries.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Guids;

namespace AbpEnterprise.Enterprises;

public class EnterpriseTypeAddress : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; private set; } = null!;
    public string Street { get; private set; } = null!;
    public string City { get; private set; } = null!;
    public string Country { get; private set; } = null!;
    public string? State { get; private set; }
    public string? ZipCode { get; private set; }
    public string? ContactPhone { get; private set; }
    public string? ContactEmail { get; private set; }
    public bool IsActive { get; private set; }
    public AddressType AddressType { get; private set; }
    
    private readonly List<EnterpriseTypeAddressMapping> _typeMappings = new();
    public IReadOnlyCollection<EnterpriseTypeAddressMapping> TypeMappings => _typeMappings.AsReadOnly();

    public void AddEnterpriseType(EnterpriseType enterpriseType)
    {
        Check.NotNull(enterpriseType, nameof(enterpriseType));
            
        if (_typeMappings.Any(x => x.EnterpriseTypeId == enterpriseType.Id))
        {
            return; // Enterprise type already added
        }

        _typeMappings.Add(new EnterpriseTypeAddressMapping(
            Guid.NewGuid(),
            enterpriseType.Id,
            this.Id
        ));
    }

    public void RemoveEnterpriseType(Guid enterpriseTypeId)
    {
        var mapping = _typeMappings.FirstOrDefault(x => x.EnterpriseTypeId == enterpriseTypeId);
        if (mapping != null)
        {
            _typeMappings.Remove(mapping);
        }
    }
    
    private EnterpriseTypeAddress() { }

    public EnterpriseTypeAddress(
        Guid id,
        string name,
        string street,
        string city,
        string state,
        string country,
        string zipCode,
        string contactPhone = null,
        string contactEmail = null,
        bool isActive = true) : base(id)
    {
        SetName(name);
        SetStreet(street);
        SetCity(city);
        SetState(state);
        SetCountry(country);
        SetZipCode(zipCode);
        SetContactPhone(contactPhone);
        SetContactEmail(contactEmail);
        SetIsActive(isActive);
    }

    public void SetName(string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), EnterpriseTypeAddressConsts.MaxNameLength);
    }

    public void SetStreet(string street)
    {
        Street = Check.NotNullOrWhiteSpace(street, nameof(street), EnterpriseTypeAddressConsts.MaxStreetLength);
    }

    public void SetCity(string city)
    {
        City = Check.NotNullOrWhiteSpace(city, nameof(city), EnterpriseTypeAddressConsts.MaxCityLength);
    }

    public void SetState(string state)
    {
        State = Check.Length(state, nameof(state), EnterpriseTypeAddressConsts.MaxStateLength);
    }

    public void SetCountry(string country)
    {
        Country = Check.NotNullOrWhiteSpace(country, nameof(country), EnterpriseTypeAddressConsts.MaxCountryLength);
    }

    public void SetZipCode(string zipCode)
    {
        ZipCode = Check.Length(zipCode, nameof(zipCode), EnterpriseTypeAddressConsts.MaxZipCodeLength);
    }

    public void SetContactPhone(string contactPhone)
    {
        ContactPhone = Check.Length(contactPhone, nameof(contactPhone), EnterpriseTypeAddressConsts.MaxContactPhoneLength);
    }

    public void SetContactEmail(string contactEmail)
    {
        ContactEmail = Check.Length(contactEmail, nameof(contactEmail), EnterpriseTypeAddressConsts.MaxContactEmailLength);
    }

    public void SetIsActive(bool isActive)
    {
        IsActive = isActive;
    }

    public string GetFullAddress()
    {
        var parts = new List<string>();

        if (!string.IsNullOrEmpty(Street)) parts.Add(Street);
        if (!string.IsNullOrEmpty(City)) parts.Add(City);
        if (!string.IsNullOrEmpty(State)) parts.Add(State);
        if (!string.IsNullOrEmpty(Country)) parts.Add(Country);
        if (!string.IsNullOrEmpty(ZipCode)) parts.Add(ZipCode);

        return string.Join(", ", parts);
    }

    public string GetContactInfo()
    {
        var parts = new List<string>();

        if (!string.IsNullOrEmpty(ContactPhone)) parts.Add($"Phone: {ContactPhone}");
        if (!string.IsNullOrEmpty(ContactEmail)) parts.Add($"Email: {ContactEmail}");

        return string.Join(" | ", parts);
    }
}