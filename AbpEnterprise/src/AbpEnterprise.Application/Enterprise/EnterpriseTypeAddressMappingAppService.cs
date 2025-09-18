using AbpEnterprise.Enterpries.EnterpriseInterfaces;
using AbpEnterprise.Enterpries.Enum;
using AbpEnterprise.Enterprises;
using AbpEnterprise.Enterprises.DomainServices;
using AbpEnterprise.Enterprises.Interfaces;
using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace AbpEnterprise.Enterprise
{
    public class EnterpriseTypeAddressMappingAppService : CrudAppService<
        EnterpriseTypeAddressMapping,
        EnterpriseTypeAddressMappingDto,
        Guid,
        GetEnterpriseTypeAddressMappingsInput,
        CreateUpdateEnterpriseTypeAddressMappingDto,
        CreateUpdateEnterpriseTypeAddressMappingDto>, IEnterpriseTypeAddressMappingAppService
    {
        private readonly IEnterpriseIndustryRepository _enterpriseIndustryRepository;
        private readonly IEnterpriseTypeAddressRepository _addressRepository;
        private readonly IEnterpriseTypeAddressMappingRepository _mappingRepository;
        private readonly EnterpriseTypeAddressMappingManager _mappingManager;

        public EnterpriseTypeAddressMappingAppService(
            IEnterpriseTypeAddressMappingRepository repository,
            IEnterpriseIndustryRepository enterpriseIndustryRepository,
            IEnterpriseTypeAddressRepository addressRepository,
            EnterpriseTypeAddressMappingManager mappingManager) : base(repository)
        {
            _enterpriseIndustryRepository = enterpriseIndustryRepository;
            _addressRepository = addressRepository;
            _mappingRepository = repository;
            _mappingManager = mappingManager;

            //GetPolicyName = YourProjectPermissions.EnterpriseTypeAddressMappings.Default;
            //GetListPolicyName = YourProjectPermissions.EnterpriseTypeAddressMappings.Default;
            //CreatePolicyName = YourProjectPermissions.EnterpriseTypeAddressMappings.Create;
            //UpdatePolicyName = YourProjectPermissions.EnterpriseTypeAddressMappings.Edit;
            //DeletePolicyName = YourProjectPermissions.EnterpriseTypeAddressMappings.Delete;
        }

        public virtual async Task<ListResultDto<EnterpriseTypeDto>> GetAvailableEnterpriseTypesAsync()
        {
            await CheckGetPolicyAsync();

            var enterpriseTypes = await _enterpriseIndustryRepository.GetAllEnterpriseTypesAsync(isActive: true);

            return new ListResultDto<EnterpriseTypeDto>(
                ObjectMapper.Map<List<EnterpriseType>, List<EnterpriseTypeDto>>(enterpriseTypes));
        }

        public virtual async Task<ListResultDto<EnterpriseTypeAddressDto>> GetAvailableAddressesAsync()
        {
            await CheckGetPolicyAsync();

            var addresses = await _addressRepository.GetListAsync(filter: null);
            var activeAddresses = addresses.Where(x => x.IsActive).ToList();

            return new ListResultDto<EnterpriseTypeAddressDto>(
                ObjectMapper.Map<List<EnterpriseTypeAddress>, List<EnterpriseTypeAddressDto>>(activeAddresses));
        }

        public virtual async Task<ListResultDto<AddressTypeDto>> GetAddressTypesAsync()
        {
            await CheckGetPolicyAsync();

            var addressTypes = Enum.GetValues<AddressType>()
                .Select(x => new AddressTypeDto
                {
                    Value = x,
                    DisplayName = x.ToString() // You can localize this
                })
                .ToList();

            return new ListResultDto<AddressTypeDto>(addressTypes);
        }

        public virtual async Task<EnterpriseTypeAddressMappingDto> CreateMappingAsync(CreateUpdateEnterpriseTypeAddressMappingDto input)
        {
            await CheckCreatePolicyAsync();

            var mapping = await _mappingManager.CreateMappingAsync(
                input.EnterpriseTypeId,
                input.EnterpriseTypeAddressId,
                input.AddressType,
                input.Notes,
                input.StartDate,
                input.EndDate,
                input.Priority);

            mapping.SetIsActive(input.IsActive);

            var savedMapping = await _mappingRepository.InsertAsync(mapping, autoSave: true);

            return await MapToGetOutputDtoAsync(savedMapping);
        }

        public virtual async Task RemoveMappingAsync(Guid mappingId)
        {
            await CheckDeletePolicyAsync();

            await _mappingManager.RemoveMappingAsync(mappingId);
        }

        
        public virtual async Task<EnterpriseTypeAddressMappingDto> ChangePriorityAsync(Guid mappingId, int newPriority)
        {
            var mapping = await _mappingManager.ChangePriorityAsync(mappingId, newPriority);
            await _mappingRepository.UpdateAsync(mapping, autoSave: true);

            return await MapToGetOutputDtoAsync(mapping);
        }

   
        public virtual async Task<EnterpriseTypeAddressMappingDto> PromoteToHeadquartersAsync(Guid mappingId)
        {
            var mapping = await _mappingManager.PromoteToHeadquartersAsync(mappingId);
            await _mappingRepository.UpdateAsync(mapping, autoSave: true);

            return await MapToGetOutputDtoAsync(mapping);
        }

       
        public virtual async Task ActivateMappingAsync(Guid mappingId, DateTime? startDate = null)
        {
            var mapping = await _mappingRepository.GetAsync(mappingId);
            mapping.Activate(startDate);
            await _mappingRepository.UpdateAsync(mapping, autoSave: true);
        }


        public virtual async Task DeactivateMappingAsync(Guid mappingId, DateTime? endDate = null)
        {
            var mapping = await _mappingRepository.GetAsync(mappingId);
            mapping.Deactivate(endDate);
            await _mappingRepository.UpdateAsync(mapping, autoSave: true);
        }

        public override async Task<EnterpriseTypeAddressMappingDto> CreateAsync(CreateUpdateEnterpriseTypeAddressMappingDto input)
        {
            return await _mappingManager.CreateMappingAsync(
                input.EnterpriseTypeId,
                input.EnterpriseTypeAddressId,
                input.AddressType,
                input.Notes,
                input.StartDate,
                input.EndDate,
                input.Priority);
        }

        protected override async Task<EnterpriseTypeAddressMapping> UpdateAsync(Guid id, CreateUpdateEnterpriseTypeAddressMappingDto input)
        {
            var mapping = await Repository.GetAsync(id);

            mapping.SetNotes(input.Notes);
            mapping.SetIsActive(input.IsActive);
            mapping.SetAddressType(input.AddressType);
            mapping.SetStartDate(input.StartDate);
            mapping.SetEndDate(input.EndDate);
            mapping.SetPriority(input.Priority);

            return await Repository.UpdateAsync(mapping, autoSave: true);
        }

        protected override async Task<IQueryable<EnterpriseTypeAddressMapping>> CreateFilteredQueryAsync(GetEnterpriseTypeAddressMappingsInput input)
        {
            var query = await ReadOnlyRepository.GetQueryableAsync();

            query = query
                .WhereIf(input.EnterpriseTypeId.HasValue, x => x.EnterpriseTypeId == input.EnterpriseTypeId)
                .WhereIf(input.EnterpriseTypeAddressId.HasValue, x => x.EnterpriseTypeAddressId == input.EnterpriseTypeAddressId)
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive)
                .WhereIf(input.AddressType.HasValue, x => x.AddressType == input.AddressType);

            if (input.IsCurrentlyActive.HasValue)
            {
                var now = DateTime.Now;
                if (input.IsCurrentlyActive.Value)
                {
                    query = query.Where(x => x.IsActive &&
                        (!x.StartDate.HasValue || x.StartDate <= now) &&
                        (!x.EndDate.HasValue || x.EndDate >= now));
                }
                else
                {
                    query = query.Where(x => !x.IsActive ||
                        (x.StartDate.HasValue && x.StartDate > now) ||
                        (x.EndDate.HasValue && x.EndDate < now));
                }
            }

            return query;
        }

        protected override async Task<EnterpriseTypeAddressMappingDto> MapToGetOutputDtoAsync(EnterpriseTypeAddressMapping entity)
        {
            var dto = await base.MapToGetOutputDtoAsync(entity);

            // Additional mapping for related entities
            var enterpriseIndustry = await _enterpriseIndustryRepository.GetByEnterpriseTypeIdAsync(entity.EnterpriseTypeId, includeDetails: true);
            var enterpriseType = enterpriseIndustry?.GetEnterpriseType(entity.EnterpriseTypeId);
            var address = await _addressRepository.FindAsync(entity.EnterpriseTypeAddressId);

            if (enterpriseType != null)
            {
                dto.EnterpriseTypeName = enterpriseType.Name;
                dto.EnterpriseTypeCode = enterpriseType.Code;
                dto.EnterpriseIndustryName = enterpriseIndustry?.Name;
            }

            if (address != null)
            {
                dto.EnterpriseTypeAddressName = address.Name;
                dto.EnterpriseTypeAddressFullAddress = address.GetFullAddress();
            }

            return dto;
        }
    }
}
