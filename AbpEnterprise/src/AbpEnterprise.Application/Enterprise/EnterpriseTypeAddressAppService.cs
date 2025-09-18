using AbpEnterprise.Enterprises;
using AbpEnterprise.Enterprises.DomainServices;
using AbpEnterprise.Enterprises.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace AbpEnterprise.Enterprise
{
    public class EnterpriseTypeAddressAppService : CrudAppService<
        EnterpriseTypeAddress,
        EnterpriseTypeAddressDto,
        Guid,
        GetEnterpriseTypeAddressesInput,
        CreateUpdateEnterpriseTypeAddressDto,
        CreateUpdateEnterpriseTypeAddressDto>, IEnterpriseTypeAddressAppService
    {
        private readonly EnterpriseTypeAddressManager _addressManager;
        private readonly IEnterpriseTypeAddressRepository _addressRepository;

        public EnterpriseTypeAddressAppService(
            IEnterpriseTypeAddressRepository repository,
            EnterpriseTypeAddressManager addressManager) : base(repository)
        {
            _addressManager = addressManager;
            _addressRepository = repository;

            //GetPolicyName = YourProjectPermissions.EnterpriseTypeAddresses.Default;
            //GetListPolicyName = YourProjectPermissions.EnterpriseTypeAddresses.Default;
            //CreatePolicyName = YourProjectPermissions.EnterpriseTypeAddresses.Create;
            //UpdatePolicyName = YourProjectPermissions.EnterpriseTypeAddresses.Edit;
            //DeletePolicyName = YourProjectPermissions.EnterpriseTypeAddresses.Delete;
        }

        public override async Task<EnterpriseTypeAddressDto> CreateAsync(CreateUpdateEnterpriseTypeAddressDto input)
        {
            var address = await _addressManager.CreateAsync(
                input.Name,
                input.Street,
                input.City,
                input.State,
                input.Country,
                input.ZipCode,
                input.ContactPhone,
                input.ContactEmail);

            address.SetIsActive(input.IsActive);
            var result = await _addressRepository.UpdateAsync(address);

            return ObjectMapper.Map<EnterpriseTypeAddress, EnterpriseTypeAddressDto>(result);
        }

        public override async Task<EnterpriseTypeAddressDto> UpdateAsync(Guid id, CreateUpdateEnterpriseTypeAddressDto input)
        {
            var address = await Repository.GetAsync(id);

            address.SetName(input.Name);
            address.SetStreet(input.Street);
            address.SetCity(input.City);
            address.SetState(input.State);
            address.SetCountry(input.Country);
            address.SetZipCode(input.ZipCode);
            address.SetContactPhone(input.ContactPhone);
            address.SetContactEmail(input.ContactEmail);
            address.SetIsActive(input.IsActive);
            var result = await Repository.UpdateAsync(address, autoSave: true);

            return  ObjectMapper.Map<EnterpriseTypeAddress, EnterpriseTypeAddressDto>(result);  
        }

        protected override async Task<IQueryable<EnterpriseTypeAddress>> CreateFilteredQueryAsync(GetEnterpriseTypeAddressesInput input)
        {
            var query = await ReadOnlyRepository.GetQueryableAsync();

            query = query
                .WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                    x => x.Name.Contains(input.Filter) ||
                         x.Street.Contains(input.Filter) ||
                         x.City.Contains(input.Filter) ||
                         x.Country.Contains(input.Filter))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);

            return query;
        }

        public virtual async Task<bool> CanDeleteAsync(Guid id)
        {
            await CheckGetPolicyAsync();
            return await _addressManager.CanDeleteAsync(id);
        }

        protected override async Task DeleteByIdAsync(Guid id)
        {
            var canDelete = await _addressManager.CanDeleteAsync(id);
            if (!canDelete)
            {
                throw new UserFriendlyException(L["CannotDeleteAddressWithActiveMappings"]);
            }

            await base.DeleteByIdAsync(id);
        }
    }
}
