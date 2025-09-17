using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace AbpEnterprise.Enterprises.Interfaces
{
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
