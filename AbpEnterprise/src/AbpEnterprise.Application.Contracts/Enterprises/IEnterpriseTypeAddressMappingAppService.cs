using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace AbpEnterprise.Enterprises
{
    public interface IEnterpriseTypeAddressMappingAppService : ICrudAppService<
        EnterpriseTypeAddressMappingDto,
        Guid,
        GetEnterpriseTypeAddressMappingsInput,
        CreateUpdateEnterpriseTypeAddressMappingDto,
        CreateUpdateEnterpriseTypeAddressMappingDto>
    {
        Task<ListResultDto<EnterpriseTypeDto>> GetAvailableEnterpriseTypesAsync();
        Task<ListResultDto<EnterpriseTypeAddressDto>> GetAvailableAddressesAsync();
        Task<ListResultDto<AddressTypeDto>> GetAddressTypesAsync();
        Task<EnterpriseTypeAddressMappingDto> CreateMappingAsync(CreateUpdateEnterpriseTypeAddressMappingDto input);
        Task RemoveMappingAsync(Guid mappingId);
        Task<EnterpriseTypeAddressMappingDto> ChangePriorityAsync(Guid mappingId, int newPriority);
        Task<EnterpriseTypeAddressMappingDto> PromoteToHeadquartersAsync(Guid mappingId);
        Task ActivateMappingAsync(Guid mappingId, DateTime? startDate = null);
        Task DeactivateMappingAsync(Guid mappingId, DateTime? endDate = null);
    }
}
