using AbpEnterprise.Enterpries.Enum;
using AbpEnterprise.Enterprises;
using AbpEnterprise.Enterprises.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace AbpEnterprise.EntityFrameworkCore.Enterprise.RepositoryImplementations
{
    public class EnterpriseTypeAddressMappingRepository : EfCoreRepository<AbpEnterpriseDbContext, EnterpriseTypeAddressMapping, Guid>, IEnterpriseTypeAddressMappingRepository
    {
        public EnterpriseTypeAddressMappingRepository(IDbContextProvider<AbpEnterpriseDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        // write code Add new EnterpriseTypeAddressMapping and save it to database
        public async Task<EnterpriseTypeAddressMapping> AddAsync(
            EnterpriseTypeAddressMapping entity,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var result = await dbContext.Set<EnterpriseTypeAddressMapping>().AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return result.Entity;
        }
        public async Task<EnterpriseTypeAddressMapping> FindByEnterpriseTypeAndAddressAsync(
            Guid enterpriseTypeId,
            Guid enterpriseTypeAddressId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query.FirstOrDefaultAsync(x =>
                x.EnterpriseTypeId == enterpriseTypeId &&
                x.EnterpriseTypeAddressId == enterpriseTypeAddressId,
                cancellationToken);
        }

        public async Task<List<EnterpriseTypeAddressMapping>> GetByEnterpriseTypeIdAsync(
            Guid enterpriseTypeId,
            bool? isActive = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            query = query
                .Where(x => x.EnterpriseTypeId == enterpriseTypeId);

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<List<EnterpriseTypeAddressMapping>> GetByAddressIdAsync(
            Guid enterpriseTypeAddressId,
            bool? isActive = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            query = query
                .Where(x => x.EnterpriseTypeAddressId == enterpriseTypeAddressId);

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<List<EnterpriseTypeAddressMapping>> GetActiveByEnterpriseTypeIdAsync(
            Guid enterpriseTypeId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            var now = DateTime.Now;

            query = query.Where(x =>
                x.EnterpriseTypeId == enterpriseTypeId);

            return await query.ToListAsync(cancellationToken);
        }      
    }
}
