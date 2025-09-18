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
                .Where(x => x.EnterpriseTypeId == enterpriseTypeId)
                .WhereIf(isActive.HasValue, x => x.IsActive == isActive.Value)
                .OrderBy(x => x.Priority)
                .ThenBy(x => x.CreationTime);

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<List<EnterpriseTypeAddressMapping>> GetByAddressIdAsync(
            Guid enterpriseTypeAddressId,
            bool? isActive = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            query = query
                .Where(x => x.EnterpriseTypeAddressId == enterpriseTypeAddressId)
                .WhereIf(isActive.HasValue, x => x.IsActive == isActive.Value)
                .OrderBy(x => x.Priority)
                .ThenBy(x => x.CreationTime);

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<List<EnterpriseTypeAddressMapping>> GetActiveByEnterpriseTypeIdAsync(
            Guid enterpriseTypeId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            var now = DateTime.Now;

            query = query.Where(x =>
                x.EnterpriseTypeId == enterpriseTypeId &&
                x.IsActive &&
                (!x.StartDate.HasValue || x.StartDate <= now) &&
                (!x.EndDate.HasValue || x.EndDate >= now))
                .OrderBy(x => x.Priority)
                .ThenBy(x => x.CreationTime);

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<EnterpriseTypeAddressMapping> FindHeadquartersByEnterpriseTypeAsync(
            Guid enterpriseTypeId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query.FirstOrDefaultAsync(x =>
                x.EnterpriseTypeId == enterpriseTypeId &&
                x.AddressType == AddressType.Headquarters &&
                x.IsActive,
                cancellationToken);
        }

        public async Task<int> GetActiveCountByEnterpriseTypeAsync(Guid enterpriseTypeId, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            var now = DateTime.Now;

            return await query.CountAsync(x =>
                x.EnterpriseTypeId == enterpriseTypeId &&
                x.IsActive &&
                (!x.StartDate.HasValue || x.StartDate <= now) &&
                (!x.EndDate.HasValue || x.EndDate >= now),
                cancellationToken);
        }

        public async Task<List<EnterpriseTypeAddressMapping>> GetListAsync(
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            Guid? enterpriseTypeId = null,
            Guid? enterpriseTypeAddressId = null,
            bool? isActive = null,
            bool? isCurrentlyActive = null,
            AddressType? addressType = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            query = query
                .WhereIf(enterpriseTypeId.HasValue, x => x.EnterpriseTypeId == enterpriseTypeId)
                .WhereIf(enterpriseTypeAddressId.HasValue, x => x.EnterpriseTypeAddressId == enterpriseTypeAddressId)
                .WhereIf(isActive.HasValue, x => x.IsActive == isActive.Value)
                .WhereIf(addressType.HasValue, x => x.AddressType == addressType.Value);

            if (isCurrentlyActive.HasValue)
            {
                var now = DateTime.Now;
                if (isCurrentlyActive.Value)
                {
                    query = query.Where(x => x.IsActive &&
                        (!x.StartDate.HasValue || x.StartDate <= now) &&
                        (!x.EndDate.HasValue || x.EndDate >= now));
                }
                else
                {
                    query = query.Where(x => !x.IsActive ||
                        (x.StartDate.HasValue && x.StartDate > now) ||
                        (x.EndDate.HasValue && x.EndDate < now));
                }
            }

            query = query
                .OrderBy(sorting.IsNullOrEmpty() ? nameof(EnterpriseTypeAddressMapping.Priority) : sorting)
                .Skip(skipCount)
                .Take(maxResultCount);

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<long> GetCountAsync(
            Guid? enterpriseTypeId = null,
            Guid? enterpriseTypeAddressId = null,
            bool? isActive = null,
            bool? isCurrentlyActive = null,
            AddressType? addressType = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            query = query
                .WhereIf(enterpriseTypeId.HasValue, x => x.EnterpriseTypeId == enterpriseTypeId)
                .WhereIf(enterpriseTypeAddressId.HasValue, x => x.EnterpriseTypeAddressId == enterpriseTypeAddressId)
                .WhereIf(isActive.HasValue, x => x.IsActive == isActive.Value)
                .WhereIf(addressType.HasValue, x => x.AddressType == addressType.Value);

            if (isCurrentlyActive.HasValue)
            {
                var now = DateTime.Now;
                if (isCurrentlyActive.Value)
                {
                    query = query.Where(x => x.IsActive &&
                        (!x.StartDate.HasValue || x.StartDate <= now) &&
                        (!x.EndDate.HasValue || x.EndDate >= now));
                }
                else
                {
                    query = query.Where(x => !x.IsActive ||
                        (x.StartDate.HasValue && x.StartDate > now) ||
                        (x.EndDate.HasValue && x.EndDate < now));
                }
            }

            return await query.CountAsync(cancellationToken);
        }
    }
}
