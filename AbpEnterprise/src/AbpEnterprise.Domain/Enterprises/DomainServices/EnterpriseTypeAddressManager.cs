using AbpEnterprise.Enterprises.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace AbpEnterprise.Enterprises.DomainServices
{
    public class EnterpriseTypeAddressManager : DomainService
    {
        private readonly IEnterpriseTypeAddressRepository _addressRepository;

        public EnterpriseTypeAddressManager(IEnterpriseTypeAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<EnterpriseTypeAddress> CreateAsync(
            string name,
            string street,
            string city,
            string state,
            string country,
            string zipCode,
            string contactPhone = null,
            string contactEmail = null)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNullOrWhiteSpace(street, nameof(street));
            Check.NotNullOrWhiteSpace(city, nameof(city));
            Check.NotNullOrWhiteSpace(country, nameof(country));

            // Check for duplicate addresses based on key fields
            var existingAddress = await _addressRepository.FindByLocationAsync(street, city, state, country, zipCode);
            if (existingAddress != null)
            {
                throw new UserFriendlyException("AddressAlreadyExists");
            }

            return new EnterpriseTypeAddress(
                GuidGenerator.Create(),
                name,
                street,
                city,
                state,
                country,
                zipCode,
                contactPhone,
                contactEmail);
        }

        public async Task<bool> CanDeleteAsync(Guid addressId)
        {
            var activeMappingsCount = await _addressRepository.GetActiveMappingsCountAsync(addressId);
            return activeMappingsCount == 0;
        }
    }
}
