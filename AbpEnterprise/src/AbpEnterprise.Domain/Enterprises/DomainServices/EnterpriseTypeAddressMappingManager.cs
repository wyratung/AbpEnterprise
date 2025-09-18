using AbpEnterprise.Enterpries.EnterpriseInterfaces;
using AbpEnterprise.Enterpries.Enum;
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
    public class EnterpriseTypeAddressMappingManager : DomainService
    {
        private readonly IEnterpriseIndustryRepository _enterpriseIndustryRepository;
        private readonly IEnterpriseTypeAddressRepository _enterpriseTypeAddressRepository;
        private readonly IEnterpriseTypeAddressMappingRepository _mappingRepository;

        public EnterpriseTypeAddressMappingManager(
            IEnterpriseIndustryRepository enterpriseIndustryRepository,
            IEnterpriseTypeAddressRepository enterpriseTypeAddressRepository,
            IEnterpriseTypeAddressMappingRepository mappingRepository)
        {
            _enterpriseIndustryRepository = enterpriseIndustryRepository;
            _enterpriseTypeAddressRepository = enterpriseTypeAddressRepository;
            _mappingRepository = mappingRepository;
        }

        public async Task<EnterpriseTypeAddressMapping> CreateMappingAsync(
            Guid enterpriseTypeId,
            Guid enterpriseTypeAddressId,
            AddressType addressType = AddressType.Office,
            string notes = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            int priority = 0)
        {
            // Verify that EnterpriseType exists (through its parent aggregate)
            var enterpriseIndustry = await _enterpriseIndustryRepository.GetByEnterpriseTypeIdAsync(enterpriseTypeId);
            if (enterpriseIndustry == null)
            {
                throw new UserFriendlyException(L["EnterpriseTypeNotFound"]);
            }

            var enterpriseType = enterpriseIndustry.GetEnterpriseType(enterpriseTypeId);
            if (enterpriseType == null)
            {
                throw new UserFriendlyException(L["EnterpriseTypeNotFound"]);
            }

            // Verify that EnterpriseTypeAddress exists (separate aggregate root)
            var address = await _enterpriseTypeAddressRepository.GetAsync(enterpriseTypeAddressId);
            if (address == null)
            {
                throw new UserFriendlyException(L["EnterpriseTypeAddressNotFound"]);
            }

            // Check if mapping already exists
            var existingMapping = await _mappingRepository.FindByEnterpriseTypeAndAddressAsync(enterpriseTypeId, enterpriseTypeAddressId);
            if (existingMapping != null)
            {
                throw new UserFriendlyException(L["MappingAlreadyExists"]);
            }

            // Business rule: Only one headquarters per enterprise type
            if (addressType == AddressType.Headquarters)
            {
                var existingHeadquarters = await _mappingRepository.FindHeadquartersByEnterpriseTypeAsync(enterpriseTypeId);
                if (existingHeadquarters != null)
                {
                    throw new UserFriendlyException(L["OnlyOneHeadquartersAllowed"]);
                }
            }

            var mapping = new EnterpriseTypeAddressMapping(
                GuidGenerator.Create(),
                enterpriseTypeId,
                enterpriseTypeAddressId,
                addressType,
                notes,
                true,
                startDate,
                endDate,
                priority);

            return mapping;
        }

        public async Task RemoveMappingAsync(Guid mappingId)
        {
            var mapping = await _mappingRepository.GetAsync(mappingId);
            if (mapping == null)
            {
                return; // Already removed
            }

            // Business rule: Cannot remove headquarters if it's the only active address
            if (mapping.AddressType == AddressType.Headquarters)
            {
                var activeMappingsCount = await _mappingRepository.GetActiveCountByEnterpriseTypeAsync(mapping.EnterpriseTypeId);
                if (activeMappingsCount <= 1)
                {
                    throw new UserFriendlyException(L["CannotRemoveLastActiveAddress"]);
                }
            }

            await _mappingRepository.DeleteAsync(mapping);
        }

        public async Task<EnterpriseTypeAddressMapping> ChangePriorityAsync(Guid mappingId, int newPriority)
        {
            var mapping = await _mappingRepository.GetAsync(mappingId);
            mapping.SetPriority(newPriority);

            return mapping;
        }

        public async Task<EnterpriseTypeAddressMapping> PromoteToHeadquartersAsync(Guid mappingId)
        {
            var mapping = await _mappingRepository.GetAsync(mappingId);

            // Remove existing headquarters
            var existingHeadquarters = await _mappingRepository.FindHeadquartersByEnterpriseTypeAsync(mapping.EnterpriseTypeId);
            if (existingHeadquarters != null && existingHeadquarters.Id != mappingId)
            {
                existingHeadquarters.SetAddressType(AddressType.Office);
                await _mappingRepository.UpdateAsync(existingHeadquarters);
            }

            mapping.SetAddressType(AddressType.Headquarters);
            mapping.SetPriority(0); // Highest priority

            return mapping;
        }
    }
}
