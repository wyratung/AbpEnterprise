using AbpEnterprise.Enterpries;
using AbpEnterprise.Enterpries.EnterpriseInterfaces;
using AbpEnterprise.Enterprises;
using AbpEnterprise.Enterprises.DomainServices;
using AutoMapper.Internal.Mappers;
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
    public class EnterpriseIndustryAppService : CrudAppService<
        EnterpriseIndustry,
        EnterpriseIndustryDto,
        Guid,
        GetEnterpriseIndustriesInput,
        CreateEnterpriseIndustryDto,
        UpdateEnterpriseIndustryDto>, IEnterpriseIndustryAppService
    {
        private readonly EnterpriseIndustryManager _enterpriseIndustryManager;
        private readonly IEnterpriseIndustryRepository _repository;

        public EnterpriseIndustryAppService(
            IEnterpriseIndustryRepository repository,
            EnterpriseIndustryManager enterpriseIndustryManager) : base(repository)
        {
            _repository = repository;
            _enterpriseIndustryManager = enterpriseIndustryManager;

            //GetPolicyName = EnterprisePermissions.EnterpriseIndustries.Default;
            //GetListPolicyName = EnterprisePermissions.EnterpriseIndustries.Default;
            //CreatePolicyName = EnterprisePermissions.EnterpriseIndustries.Create;
            //UpdatePolicyName = EnterprisePermissions.EnterpriseIndustries.Update;
            //DeletePolicyName = EnterprisePermissions.EnterpriseIndustries.Delete;
        }

        public async Task<EnterpriseIndustryDto> GetWithDetailsAsync(Guid id)
        {
            await CheckGetPolicyAsync();

            var entity = await _repository.GetAsync(id, includeDetails: true);
            return await MapToGetOutputDtoAsync(entity);
        }

        protected override async Task<IQueryable<EnterpriseIndustry>> CreateFilteredQueryAsync(GetEnterpriseIndustriesInput input)
        {
            var query = await ReadOnlyRepository.GetQueryableAsync();

            return query
                .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Filter) || x.Code.Contains(input.Filter))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive.Value);
        }

        public override async Task<EnterpriseIndustryDto> CreateAsync(CreateEnterpriseIndustryDto input)
        {
            await CheckCreatePolicyAsync();

            var entity = await _enterpriseIndustryManager.CreateAsync(
                input.Name,
                input.Code,
                input.Description
            );

            await Repository.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            return await MapToGetOutputDtoAsync(entity);
        }

        public override async Task<EnterpriseIndustryDto> UpdateAsync(Guid id, UpdateEnterpriseIndustryDto input)
        {
            await CheckUpdatePolicyAsync();

            var entity = await Repository.GetAsync(id);

            entity.SetName(input.Name);
            entity.SetDescription(input.Description);

            await Repository.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            return await MapToGetOutputDtoAsync(entity);
        }

        public async Task ActivateAsync(Guid id)
        {
            await CheckUpdatePolicyAsync();

            var entity = await Repository.GetAsync(id);
            entity.Activate();
            await Repository.UpdateAsync(entity);
        }

        public async Task DeactivateAsync(Guid id)
        {
            await CheckUpdatePolicyAsync();

            var entity = await Repository.GetAsync(id, includeDetails: true);
            entity.Deactivate();
            await Repository.UpdateAsync(entity);
        }

        public async Task<List<EnterpriseIndustryDto>> GetActiveListAsync()
        {
            await CheckGetPolicyAsync();

            var entities = await _repository.GetListAsync(isActive: true, sorting: nameof(EnterpriseIndustry.Name));
            return ObjectMapper.Map<List<EnterpriseIndustry>, List<EnterpriseIndustryDto>>(entities);
        }
    }

}
