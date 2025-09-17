//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Volo.Abp.Application.Services;

//namespace AbpEnterprise.Enterprises
//{
//    public interface IEnterpriseTypeAppService : ICrudAppService<
//        EnterpriseTypeDto,
//        Guid,
//        GetEnterpriseTypesInput,
//        CreateEnterpriseTypeDto,
//        UpdateEnterpriseTypeDto>
//    {
//        Task ActivateAsync(Guid id);
//        Task DeactivateAsync(Guid id);
//        Task<List<EnterpriseTypeDto>> GetByIndustryIdAsync(Guid industryId);
//    }
//}
