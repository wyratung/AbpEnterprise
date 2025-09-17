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
        Task<EnterpriseIndustryDto> GetWithDetailsAsync(Guid id);
        Task ActivateAsync(Guid id);
        Task DeactivateAsync(Guid id);
        Task<List<EnterpriseIndustryDto>> GetActiveListAsync();
    }
}
