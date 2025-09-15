using AbpEnterprise.Books;
using AbpEnterprise.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;
using System.Linq.Dynamic.Core;

namespace AbpEnterprise.NewFolder
{
    public class EnterpriseTypeAppService : ApplicationService,IEnterpriseTypeAppService
    {
        private readonly IRepository<EnterpriseType, Guid> _enterpriseTypeRepository;

        public EnterpriseTypeAppService(IRepository<EnterpriseType, Guid> repository)
        {
            _enterpriseTypeRepository = repository;
        }
        

        public async Task<EnterpriseTypeDto> CreateAsync(CreateEnterpriseTypeDto input)
        {
            var enterpriseType = new EnterpriseType(GuidGenerator.Create(), input.Name, input.Description);
            await _enterpriseTypeRepository.InsertAsync(enterpriseType);
            return ObjectMapper.Map<EnterpriseType, EnterpriseTypeDto>(enterpriseType);
        }

        public async Task<EnterpriseTypeDto> GetAsync(Guid id)
        {
            var enterpriseType = await _enterpriseTypeRepository.GetAsync(id);
            return ObjectMapper.Map<EnterpriseType, EnterpriseTypeDto>(enterpriseType);

        }

        public async Task<EnterpriseTypeDto> UpdateAsync(Guid id, UpdateEnterpriseTypeDto input)
        {
            var enterpriseType = await _enterpriseTypeRepository.GetAsync(id);
            enterpriseType.Update(input.Name, input.Description);
            await _enterpriseTypeRepository.UpdateAsync(enterpriseType);
            return ObjectMapper.Map<EnterpriseType, EnterpriseTypeDto>(enterpriseType);
        }

        public async Task AddIndustryAsync(Guid enterpriseTypeId, CreateEnterpriseIndustrieDto input)
        {
            var enterpriseType = await _enterpriseTypeRepository.GetAsync(enterpriseTypeId);
            enterpriseType.AddIndustry(GuidGenerator.Create(), input.Name, input.Code);
            await _enterpriseTypeRepository.UpdateAsync(enterpriseType);
        }

        public async Task<PagedResultDto<EnterpriseTypeDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var queryable = await _enterpriseTypeRepository.GetQueryableAsync();
            var query = queryable
                .OrderBy(input.Sorting.IsNullOrWhiteSpace() ? "Name" : input.Sorting)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var enterpriseTypes = await AsyncExecuter.ToListAsync(query);
            var totalCount = await AsyncExecuter.CountAsync(queryable);

            return new PagedResultDto<EnterpriseTypeDto>(
                totalCount,
                ObjectMapper.Map<List<EnterpriseType>, List<EnterpriseTypeDto>>(enterpriseTypes)
            );

        }

        public async Task<EnterpriseTypeDto> UpdateAsync(Guid id, CreateEnterpriseTypeDto input)
        {
            var enterpriseType = await _enterpriseTypeRepository.GetAsync(id);
            enterpriseType.Update(input.Name, input.Description);
            await _enterpriseTypeRepository.UpdateAsync(enterpriseType);
            return ObjectMapper.Map<EnterpriseType, EnterpriseTypeDto>(enterpriseType);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _enterpriseTypeRepository.DeleteAsync(id);
        }
    }
}
