using AbpEnterprise.Enterprises;
using AbpEnterprise.RepositoryInterface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace AbpEnterprise.EntityFrameworkCore.Enterprise
{
    public class EfCoreEnterpriseTypeRepository : EfCoreRepository<AbpEnterpriseDbContext, EnterpriseType, Guid>, IEnterpriseTypeRepository
    {
        public EfCoreEnterpriseTypeRepository(IDbContextProvider<AbpEnterpriseDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<EnterpriseType> FindByNameAsync(string name)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Include(e => e.Industries) 
                .FirstOrDefaultAsync(e => e.Name == name);
        }
    }
}
