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
}
