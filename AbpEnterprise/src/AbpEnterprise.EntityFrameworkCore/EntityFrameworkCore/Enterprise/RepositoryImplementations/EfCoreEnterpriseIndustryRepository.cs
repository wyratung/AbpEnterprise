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
    public class EfCoreEnterpriseIndustryRepository : EfCoreRepository<AbpEnterpriseDbContext, EnterpriseIndustry, Guid>, IEnterpriseIndustryRepository
    {
        public EfCoreEnterpriseIndustryRepository(IDbContextProvider<AbpEnterpriseDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<EnterpriseIndustry> FindByCodeAsync(string code)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<List<EnterpriseIndustry>> GetListAsync(
            string filter = null,
            bool? isActive = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            bool includeDetails = false)
        {
            var dbSet = await GetDbSetAsync();
            var query = dbSet
                .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Name.Contains(filter) || x.Code.Contains(filter))
                .WhereIf(isActive.HasValue, x => x.IsActive == isActive.Value);

            if (includeDetails)
            {
                query = query.Include(x => x.EnterpriseTypes);
            }

            return await query
                .OrderBy(x => x.Name)
                .PageBy(skipCount, maxResultCount)
                .ToListAsync();
        }

        public async Task<long> GetCountAsync(string filter = null, bool? isActive = null)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Name.Contains(filter) || x.Code.Contains(filter))
                .WhereIf(isActive.HasValue, x => x.IsActive == isActive.Value)
                .CountAsync();
        }

        public override async Task<IQueryable<EnterpriseIndustry>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).Include(x => x.EnterpriseTypes);
        }
    }
}
