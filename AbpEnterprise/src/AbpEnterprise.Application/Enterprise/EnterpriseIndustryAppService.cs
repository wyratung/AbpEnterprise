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

        // ===== INDUSTRY OPERATIONS =====
        protected override async Task<IQueryable<EnterpriseIndustry>> CreateFilteredQueryAsync(GetEnterpriseIndustriesInput input)
        {
            var query = await ReadOnlyRepository.GetQueryableAsync();

            //if (input.IncludeTypes)
            //{
            //    query = query.Include(x => x.EnterpriseTypes);
            //}

            return query
                .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Filter) || x.Code.Contains(input.Filter))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive.Value);
        }

        public async Task<EnterpriseIndustryDto> GetWithTypesAsync(Guid id)
        {
            await CheckGetPolicyAsync();

            var entity = await _repository.GetAsync(id, includeDetails: true);
            return await MapToGetOutputDtoAsync(entity);
        }

        public override async Task<EnterpriseIndustryDto> CreateAsync(CreateEnterpriseIndustryDto input)
        {
            await CheckCreatePolicyAsync();

            var entity = await _enterpriseIndustryManager.CreateAsync(
                input.Name,
                input.Code,
                input.Description
            );

            // Tạo EnterpriseTypes nếu có
            foreach (var typeInput in input.EnterpriseTypes)
            {
                await _enterpriseIndustryManager.CreateEnterpriseTypeAsync(
                    entity,
                    typeInput.Name,
                    typeInput.Code,
                    typeInput.Description);
            }

            await Repository.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            return await MapToGetOutputDtoAsync(entity);
        }

        public async Task ActivateIndustryAsync(Guid id)
        {
            await CheckUpdatePolicyAsync();

            var entity = await Repository.GetAsync(id);
            entity.Activate();
            await Repository.UpdateAsync(entity);
        }

        public async Task DeactivateIndustryAsync(Guid id)
        {
            await CheckUpdatePolicyAsync();

            var entity = await Repository.GetAsync(id, includeDetails: true);
            entity.Deactivate();
            await Repository.UpdateAsync(entity);
        }

        public async Task<List<EnterpriseIndustryDto>> GetActiveIndustriesAsync()
        {
            await CheckGetPolicyAsync();

            var entities = await _repository.GetListAsync(isActive: true, sorting: nameof(EnterpriseIndustry.Name));
            return ObjectMapper.Map<List<EnterpriseIndustry>, List<EnterpriseIndustryDto>>(entities);
        }

        // ===== ENTERPRISE TYPE OPERATIONS - VIA AGGREGATE ROOT =====
        public async Task<EnterpriseTypeDto> AddEnterpriseTypeAsync(Guid industryId, CreateEnterpriseTypeDto input)
        {
            await CheckCreatePolicyAsync(); // Sử dụng permission của Industry

            var industry = await _repository.GetAsync(industryId, includeDetails: true);

            var enterpriseType = await _enterpriseIndustryManager.CreateEnterpriseTypeAsync(
                industry,
                input.Name,
                input.Code,
                input.Description
            );

            await _repository.UpdateAsync(industry);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<EnterpriseType, EnterpriseTypeDto>(enterpriseType);
        }

        public async Task<EnterpriseTypeDto> UpdateEnterpriseTypeAsync(Guid industryId, Guid typeId, UpdateEnterpriseTypeDto input)
        {
            await CheckUpdatePolicyAsync();

            var industry = await _repository.GetAsync(industryId, includeDetails: true);
            var enterpriseType = industry.GetEnterpriseType(typeId);

            if (enterpriseType == null)
            {
                throw new BusinessException(EnterpriseDomainErrorCodes.EnterpriseTypeNotFound);
            }

            enterpriseType.SetName(input.Name);
            enterpriseType.SetDescription(input.Description);

            await _repository.UpdateAsync(industry);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<EnterpriseType, EnterpriseTypeDto>(enterpriseType);
        }

        public async Task RemoveEnterpriseTypeAsync(Guid industryId, Guid typeId)
        {
            await CheckDeletePolicyAsync();

            var industry = await _repository.GetAsync(industryId, includeDetails: true);
            industry.RemoveEnterpriseType(typeId);

            await _repository.UpdateAsync(industry);
        }

        public async Task ActivateEnterpriseTypeAsync(Guid industryId, Guid typeId)
        {
            await CheckUpdatePolicyAsync();

            var industry = await _repository.GetAsync(industryId, includeDetails: true);
            var enterpriseType = industry.GetEnterpriseType(typeId);

            if (enterpriseType == null)
            {
                throw new BusinessException(EnterpriseDomainErrorCodes.EnterpriseTypeNotFound);
            }

            enterpriseType.Activate();
            await _repository.UpdateAsync(industry);
        }

        public async Task DeactivateEnterpriseTypeAsync(Guid industryId, Guid typeId)
        {
            await CheckUpdatePolicyAsync();

            var industry = await _repository.GetAsync(industryId, includeDetails: true);
            var enterpriseType = industry.GetEnterpriseType(typeId);

            if (enterpriseType == null)
            {
                throw new BusinessException(EnterpriseDomainErrorCodes.EnterpriseTypeNotFound);
            }

            enterpriseType.Deactivate();
            await _repository.UpdateAsync(industry);
        }

        public async Task<List<EnterpriseTypeDto>> GetEnterpriseTypesByIndustryAsync(Guid industryId)
        {
            await CheckGetPolicyAsync();

            var industry = await _repository.GetAsync(industryId, includeDetails: true);
            return ObjectMapper.Map<List<EnterpriseType>, List<EnterpriseTypeDto>>(
                industry.EnterpriseTypes.OrderBy(x => x.Name).ToList());
        }

        public async Task<EnterpriseTypeDto> GetEnterpriseTypeAsync(Guid industryId, Guid typeId)
        {
            await CheckGetPolicyAsync();

            var industry = await _repository.GetAsync(industryId, includeDetails: true);
            var enterpriseType = industry.GetEnterpriseType(typeId);

            if (enterpriseType == null)
            {
                throw new BusinessException(EnterpriseDomainErrorCodes.EnterpriseTypeNotFound);
            }

            return ObjectMapper.Map<EnterpriseType, EnterpriseTypeDto>(enterpriseType);
        }

        // ===== BULK OPERATIONS =====
        public async Task<List<EnterpriseTypeDto>> AddMultipleEnterpriseTypesAsync(Guid industryId, List<CreateEnterpriseTypeDto> inputs)
        {
            await CheckCreatePolicyAsync();

            var industry = await _repository.GetAsync(industryId, includeDetails: true);
            var results = new List<EnterpriseType>();

            foreach (var input in inputs)
            {
                var enterpriseType = await _enterpriseIndustryManager.CreateEnterpriseTypeAsync(
                    industry,
                    input.Name,
                    input.Code,
                    input.Description
                );
                results.Add(enterpriseType);
            }

            await _repository.UpdateAsync(industry);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<List<EnterpriseType>, List<EnterpriseTypeDto>>(results);
        }       
    }
}
