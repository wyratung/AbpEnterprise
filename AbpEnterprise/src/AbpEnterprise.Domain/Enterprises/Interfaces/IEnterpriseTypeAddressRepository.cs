using AbpEnterprise.Enterpries.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace AbpEnterprise.Enterprises.Interfaces
{
    public interface IEnterpriseTypeAddressRepository : IRepository<EnterpriseTypeAddress, Guid>
    {
        Task<List<EnterpriseTypeAddress>> GetListAsync(
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string filter = null,
            CancellationToken cancellationToken = default);

        Task<long> GetCountAsync(
            string filter = null,
            CancellationToken cancellationToken = default);

        Task<EnterpriseTypeAddress> FindByLocationAsync(
            string street,
            string city,
            string state,
            string country,
            string zipCode,
            CancellationToken cancellationToken = default);

        Task<int> GetActiveMappingsCountAsync(
            Guid addressId,
            CancellationToken cancellationToken = default);

        Task<List<EnterpriseTypeAddress>> GetByEnterpriseTypeIdAsync(
            Guid enterpriseTypeId,
            CancellationToken cancellationToken = default);
    }
    public interface IEnterpriseTypeAddressMappingRepository : IRepository<EnterpriseTypeAddressMapping, Guid>
    {
        //    Task<EnterpriseTypeAddressMapping> FindByEnterpriseTypeAndAddressAsync(
        //        Guid enterpriseTypeId,
        //        Guid enterpriseTypeAddressId,
        //        CancellationToken cancellationToken = default);

        //    Task<List<EnterpriseTypeAddressMapping>> GetByEnterpriseTypeIdAsync(
        //        Guid enterpriseTypeId,
        //        bool? isActive = null,
        //        CancellationToken cancellationToken = default);

        //    Task<List<EnterpriseTypeAddressMapping>> GetByAddressIdAsync(
        //        Guid enterpriseTypeAddressId,
        //        bool? isActive = null,
        //        CancellationToken cancellationToken = default);

        //    Task<List<EnterpriseTypeAddressMapping>> GetActiveByEnterpriseTypeIdAsync(
        //        Guid enterpriseTypeId,
        //        CancellationToken cancellationToken = default);

        //    Task<EnterpriseTypeAddressMapping> FindHeadquartersByEnterpriseTypeAsync(
        //        Guid enterpriseTypeId,
        //        CancellationToken cancellationToken = default);

        //    Task<int> GetActiveCountByEnterpriseTypeAsync(
        //        Guid enterpriseTypeId,
        //        CancellationToken cancellationToken = default);

        //    Task<List<EnterpriseTypeAddressMapping>> GetListAsync(
        //        string sorting = null,
        //        int maxResultCount = int.MaxValue,
        //        int skipCount = 0,
        //        Guid? enterpriseTypeId = null,
        //        Guid? enterpriseTypeAddressId = null,
        //        bool? isActive = null,
        //        bool? isCurrentlyActive = null,
        //        AddressType? addressType = null,
        //        CancellationToken cancellationToken = default);

        //    Task<long> GetCountAsync(
        //        Guid? enterpriseTypeId = null,
        //        Guid? enterpriseTypeAddressId = null,
        //        bool? isActive = null,
        //        bool? isCurrentlyActive = null,
        //        AddressType? addressType = null,
        //        CancellationToken cancellationToken = default);
        Task<EnterpriseTypeAddressMapping> AddAsync(
           EnterpriseTypeAddressMapping entity,
           CancellationToken cancellationToken = default);
    }
}
