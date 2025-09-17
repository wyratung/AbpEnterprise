using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace AbpEnterprise.Enterprises
{
    public interface IEnterpriseIndustryAppService : ICrudAppService<
         EnterpriseIndustryDto,
         Guid,
         GetEnterpriseIndustriesInput,
         CreateEnterpriseIndustryDto,
         UpdateEnterpriseIndustryDto>
    {
        // Industry operations
        Task<EnterpriseIndustryDto> GetWithTypesAsync(Guid id);
        Task ActivateIndustryAsync(Guid id);
        Task DeactivateIndustryAsync(Guid id);
        Task<List<EnterpriseIndustryDto>> GetActiveIndustriesAsync();

        // EnterpriseType operations - VIA AGGREGATE ROOT
        Task<EnterpriseTypeDto> AddEnterpriseTypeAsync(Guid industryId, CreateEnterpriseTypeDto input);
        Task<EnterpriseTypeDto> UpdateEnterpriseTypeAsync(Guid industryId, Guid typeId, UpdateEnterpriseTypeDto input);
        Task RemoveEnterpriseTypeAsync(Guid industryId, Guid typeId);
        Task ActivateEnterpriseTypeAsync(Guid industryId, Guid typeId);
        Task DeactivateEnterpriseTypeAsync(Guid industryId, Guid typeId);
        Task<List<EnterpriseTypeDto>> GetEnterpriseTypesByIndustryAsync(Guid industryId);
        Task<EnterpriseTypeDto> GetEnterpriseTypeAsync(Guid industryId, Guid typeId);

        // Bulk operations
        Task<List<EnterpriseTypeDto>> AddMultipleEnterpriseTypesAsync(Guid industryId, List<CreateEnterpriseTypeDto> inputs);
        
    }
}
