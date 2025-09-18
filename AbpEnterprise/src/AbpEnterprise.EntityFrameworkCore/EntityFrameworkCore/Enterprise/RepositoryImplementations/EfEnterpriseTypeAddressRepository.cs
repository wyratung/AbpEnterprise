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
using System.Linq.Dynamic.Core;

namespace AbpEnterprise.EntityFrameworkCore.Enterprise.RepositoryImplementations
{
    public class EnterpriseTypeAddressRepository : EfCoreRepository<AbpEnterpriseDbContext, EnterpriseTypeAddress, Guid>, IEnterpriseTypeAddressRepository
    {
        public EnterpriseTypeAddressRepository(IDbContextProvider<AbpEnterpriseDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<EnterpriseTypeAddress>> GetListAsync(
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            query = query
                .WhereIf(!filter.IsNullOrWhiteSpace(), x =>
                    x.Name.Contains(filter) ||
                    x.Street.Contains(filter) ||
                    x.City.Contains(filter) ||
                    x.Country.Contains(filter))
                .OrderBy(sorting.IsNullOrEmpty() ? nameof(EnterpriseTypeAddress.Name) : sorting)
                .Skip(skipCount)
                .Take(maxResultCount);

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            query = query.WhereIf(!filter.IsNullOrWhiteSpace(), x =>
                x.Name.Contains(filter) ||
                x.Street.Contains(filter) ||
                x.City.Contains(filter) ||
                x.Country.Contains(filter));

            return await query.CountAsync(cancellationToken);
        }

        public async Task<EnterpriseTypeAddress> FindByLocationAsync(
            string street,
            string city,
            string state,
            string country,
            string zipCode,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query.FirstOrDefaultAsync(x =>
                x.Street == street &&
                x.City == city &&
                x.State == state &&
                x.Country == country &&
                x.ZipCode == zipCode,
                cancellationToken);
        }

        public async Task<int> GetActiveMappingsCountAsync(Guid addressId, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return await dbContext.Set<EnterpriseTypeAddressMapping>()
                .Where(x => x.EnterpriseTypeAddressId == addressId)
                .CountAsync(cancellationToken);
        }

        public async Task<List<EnterpriseTypeAddress>> GetByEnterpriseTypeIdAsync(Guid enterpriseTypeId, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            var mappingQuery = dbContext.Set<EnterpriseTypeAddressMapping>()
                .Where(x => x.EnterpriseTypeId == enterpriseTypeId)
                .Select(x => x.EnterpriseTypeAddressId);

            var query = await GetQueryableAsync();
            return await query.Where(x => mappingQuery.Contains(x.Id)).ToListAsync(cancellationToken);
        }
    }
}
