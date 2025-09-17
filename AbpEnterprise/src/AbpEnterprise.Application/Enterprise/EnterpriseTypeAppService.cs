//using AbpEnterprise.Books;
//using AbpEnterprise.Enterpries.EnterpriseInterfaces;
//using AbpEnterprise.Enterprises;
//using AbpEnterprise.Enterprises.DomainServices;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Dynamic.Core;
//using System.Text;
//using System.Threading.Tasks;
//using Volo.Abp;
//using Volo.Abp.Application.Dtos;
//using Volo.Abp.Application.Services;
//using Volo.Abp.Domain.Repositories;
//using Volo.Abp.Guids;
//using Volo.Abp.ObjectMapping;

//namespace AbpEnterprise.NewFolder
//{
//    [RemoteService(IsEnabled = false)]
//    public class EnterpriseTypeAppService : CrudAppService<
//        EnterpriseType,
//        EnterpriseTypeDto,
//        Guid,
//        GetEnterpriseTypesInput,
//        CreateEnterpriseTypeDto,
//        UpdateEnterpriseTypeDto>, IEnterpriseTypeAppService
//    {
//        private readonly EnterpriseIndustryManager _enterpriseIndustryManager;
//        private readonly IEnterpriseTypeRepository _enterpriseTypeRepository;
//        private readonly IEnterpriseIndustryRepository _enterpriseIndustryRepository;

//        public EnterpriseTypeAppService(
//            IEnterpriseTypeRepository repository,
//            EnterpriseIndustryManager enterpriseIndustryManager,
//            IEnterpriseIndustryRepository enterpriseIndustryRepository) : base(repository)
//        {
//            _enterpriseTypeRepository = repository;
//            _enterpriseIndustryManager = enterpriseIndustryManager;
//            _enterpriseIndustryRepository = enterpriseIndustryRepository;

//            //GetPolicyName = EnterprisePermissions.EnterpriseTypes.Default;
//            //GetListPolicyName = EnterprisePermissions.EnterpriseTypes.Default;
//            //CreatePolicyName = EnterprisePermissions.EnterpriseTypes.Create;
//            //UpdatePolicyName = EnterprisePermissions.EnterpriseTypes.Update;
//            //DeletePolicyName = EnterprisePermissions.EnterpriseTypes.Delete;
//        }

//        protected override async Task<IQueryable<EnterpriseType>> CreateFilteredQueryAsync(GetEnterpriseTypesInput input)
//        {
//            var query = await ReadOnlyRepository.GetQueryableAsync();

//            return query
//                .WhereIf(input.EnterpriseIndustryId.HasValue, x => x.EnterpriseIndustryId == input.EnterpriseIndustryId.Value)
//                .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Filter) || x.Code.Contains(input.Filter))
//                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive.Value);
//        }

//        public override async Task<EnterpriseTypeDto> CreateAsync(CreateEnterpriseTypeDto input)
//        {
//            await CheckCreatePolicyAsync();

//            var industry = await _enterpriseIndustryRepository.GetAsync(input.EnterpriseIndustryId);

//            var entity = await _enterpriseIndustryManager.CreateEnterpriseTypeAsync(
//                industry,
//                input.Name,
//                input.Code,
//                input.Description
//            );

//            await _enterpriseIndustryRepository.UpdateAsync(industry);
//            await CurrentUnitOfWork.SaveChangesAsync();

//            return await MapToGetOutputDtoAsync(entity);
//        }

//        public override async Task<EnterpriseTypeDto> UpdateAsync(Guid id, UpdateEnterpriseTypeDto input)
//        {
//            await CheckUpdatePolicyAsync();

//            var entity = await Repository.GetAsync(id);

//            entity.SetName(input.Name);
//            entity.SetDescription(input.Description);            

//            await Repository.UpdateAsync(entity);
//            await CurrentUnitOfWork.SaveChangesAsync();

//            return await MapToGetOutputDtoAsync(entity);
//        }

//        public async Task ActivateAsync(Guid id)
//        {
//            await CheckUpdatePolicyAsync();

//            var entity = await Repository.GetAsync(id);
//            entity.Activate();
//            await Repository.UpdateAsync(entity);
//        }

//        public async Task DeactivateAsync(Guid id)
//        {
//            await CheckUpdatePolicyAsync();

//            var entity = await Repository.GetAsync(id);
//            entity.Deactivate();
//            await Repository.UpdateAsync(entity);
//        }

//        public async Task<List<EnterpriseTypeDto>> GetByIndustryIdAsync(Guid industryId)
//        {
//            await CheckGetPolicyAsync();

//            var entities = await _enterpriseTypeRepository.GetListByIndustryIdAsync(industryId);
//            return ObjectMapper.Map<List<EnterpriseType>, List<EnterpriseTypeDto>>(entities);
//        }
//    }
//}
