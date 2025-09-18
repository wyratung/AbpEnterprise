using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AbpEnterprise.Enterprises
{
    public interface IEnterpriseTypeAddressAppService : ICrudAppService<
        EnterpriseTypeAddressDto,
        Guid,
        GetEnterpriseTypeAddressesInput,
        CreateUpdateEnterpriseTypeAddressDto,
        CreateUpdateEnterpriseTypeAddressDto>
    {
        Task<bool> CanDeleteAsync(Guid id);
    }
}
