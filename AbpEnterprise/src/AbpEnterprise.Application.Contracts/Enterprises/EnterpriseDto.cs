using AbpEnterprise.Enterpries.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace AbpEnterprise.Enterprises
{
    public class EnterpriseIndustryDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public List<EnterpriseTypeDto> EnterpriseTypes { get; set; } = new();
    }

    public class EnterpriseTypeDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public Guid EnterpriseIndustryId { get; set; }
    }

    // Create/Update DTOs cho Industry
    public class CreateEnterpriseIndustryDto
    {
        [Required]
        [StringLength(EnterpriseIndustryConsts.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(EnterpriseIndustryConsts.MaxCodeLength)]
        public string Code { get; set; }

        [StringLength(EnterpriseIndustryConsts.MaxDescriptionLength)]
        public string Description { get; set; }
        public List<CreateEnterpriseTypeDto> EnterpriseTypes { get; set; } = new();
    }

    public class UpdateEnterpriseIndustryDto
    {
        [Required]
        [StringLength(EnterpriseIndustryConsts.MaxNameLength)]
        public string Name { get; set; }

        [StringLength(EnterpriseIndustryConsts.MaxDescriptionLength)]
        public string Description { get; set; }

        public int SortOrder { get; set; }
    }

    // DTOs cho EnterpriseType - chỉ được sử dụng thông qua Industry
    public class CreateEnterpriseTypeDto
    {
        [Required]
        [StringLength(EnterpriseTypeConsts.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(EnterpriseTypeConsts.MaxCodeLength)]
        public string Code { get; set; }

        [StringLength(EnterpriseTypeConsts.MaxDescriptionLength)]
        public string Description { get; set; }

        public int SortOrder { get; set; }
    }

    public class UpdateEnterpriseTypeDto
    {
        [Required]
        [StringLength(EnterpriseTypeConsts.MaxNameLength)]
        public string Name { get; set; }

        [StringLength(EnterpriseTypeConsts.MaxDescriptionLength)]
        public string Description { get; set; }
    }

    // Get List DTOs
    public class GetEnterpriseIndustriesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public bool? IsActive { get; set; }
        public bool IncludeTypes { get; set; } = false;
    }

    public class CreateUpdateEnterpriseTypeDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public Guid EnterpriseIndustryId { get; set; }
    }

    public class GetEnterpriseTypesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public Guid? EnterpriseIndustryId { get; set; }
        public bool? IsActive { get; set; }
    }


    public class EnterpriseTypeAddressDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public bool IsActive { get; set; }
        public string FullAddress { get; set; }
        public string ContactInfo { get; set; }
        public int MappingCount { get; set; }
        public int ActiveMappingCount { get; set; }
    }

    public class CreateUpdateEnterpriseTypeAddressDto
    {
        public string Name { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class GetEnterpriseTypeAddressesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public bool? IsActive { get; set; }
    }

    public class EnterpriseTypeAddressMappingDto : FullAuditedEntityDto<Guid>
    {
        public Guid EnterpriseTypeId { get; set; }
        public string EnterpriseTypeName { get; set; }
        public string EnterpriseTypeCode { get; set; }
        public string EnterpriseIndustryName { get; set; }

        public Guid EnterpriseTypeAddressId { get; set; }
        public string EnterpriseTypeAddressName { get; set; }
        public string EnterpriseTypeAddressFullAddress { get; set; }

        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public AddressType AddressType { get; set; }
        public string AddressTypeDisplayName { get; set; }
        public int Priority { get; set; }
        public bool IsCurrentlyActive { get; set; }
    }

    public class CreateUpdateEnterpriseTypeAddressMappingDto
    {
        public Guid EnterpriseTypeId { get; set; }
        public Guid EnterpriseTypeAddressId { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public AddressType AddressType { get; set; } = AddressType.Office;
        public int Priority { get; set; } = 0;
    }

    public class GetEnterpriseTypeAddressMappingsInput : PagedAndSortedResultRequestDto
    {
        public Guid? EnterpriseTypeId { get; set; }
        public Guid? EnterpriseTypeAddressId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsCurrentlyActive { get; set; }
        public AddressType? AddressType { get; set; }
    }

    public class AddressTypeDto
    {
        public AddressType Value { get; set; }
        public string DisplayName { get; set; }
    }
}
