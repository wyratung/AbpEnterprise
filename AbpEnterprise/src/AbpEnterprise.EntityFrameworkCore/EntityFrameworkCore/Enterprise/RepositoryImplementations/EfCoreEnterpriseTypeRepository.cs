using AbpEnterprise.Enterpries.EnterpriseInterfaces;
using AbpEnterprise.Enterprises;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace AbpEnterprise.EntityFrameworkCore.Enterprise.RepositoryImplementations
{
    public class EfCoreEnterpriseTypeRepository : EfCoreRepository<AbpEnterpriseDbContext, EnterpriseType, Guid>, IEnterpriseTypeRepository
    {
        public EfCoreEnterpriseTypeRepository(IDbContextProvider<AbpEnterpriseDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<EnterpriseType>> GetListAsync(
            Guid? industryId = null,
            string filter = null,
            bool? isActive = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .WhereIf(industryId.HasValue, x => x.EnterpriseIndustryId == industryId.Value)
                .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Name.Contains(filter) || x.Code.Contains(filter))
                .WhereIf(isActive.HasValue, x => x.IsActive == isActive.Value)
                .PageBy(skipCount, maxResultCount)
                .ToListAsync();
        }

        public async Task<long> GetCountAsync(Guid? industryId = null, string filter = null, bool? isActive = null)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .WhereIf(industryId.HasValue, x => x.EnterpriseIndustryId == industryId.Value)
                .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Name.Contains(filter) || x.Code.Contains(filter))
                .WhereIf(isActive.HasValue, x => x.IsActive == isActive.Value)
                .CountAsync();
        }

        public async Task<List<EnterpriseType>> GetListByIndustryIdAsync(Guid industryId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(x => x.EnterpriseIndustryId == industryId)
                .ToListAsync();
        }
    }
}

