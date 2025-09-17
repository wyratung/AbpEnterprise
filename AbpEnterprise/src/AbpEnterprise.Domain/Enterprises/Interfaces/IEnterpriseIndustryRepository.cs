using AbpEnterprise.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace AbpEnterprise.Enterpries.EnterpriseInterfaces
{
    public interface IEnterpriseIndustryRepository : IRepository<EnterpriseIndustry, Guid>
    {
        Task<EnterpriseIndustry> FindByCodeAsync(string code);
        Task<List<EnterpriseIndustry>> GetListAsync(
            string filter = null,
            bool? isActive = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            bool includeDetails = false);
        Task<long> GetCountAsync(string filter = null, bool? isActive = null);
    }

    public interface IEnterpriseTypeRepository : IRepository<EnterpriseType, Guid>
    {
        Task<List<EnterpriseType>> GetListAsync(
            Guid? industryId = null,
            string filter = null,
            bool? isActive = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0);
        Task<long> GetCountAsync(Guid? industryId = null, string filter = null, bool? isActive = null);
        Task<List<EnterpriseType>> GetListByIndustryIdAsync(Guid industryId);
    }
}
