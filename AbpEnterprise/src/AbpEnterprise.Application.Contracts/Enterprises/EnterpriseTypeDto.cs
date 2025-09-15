using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace AbpEnterprise.Enterprises
{
    public class EnterpriseTypeDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<EnterpriseIndustrieDto> Industries { get; set; }
    }

    public class CreateEnterpriseTypeDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UpdateEnterpriseTypeDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class  EnterpriseIndustrieDto : EntityDto<Guid>
    {
        public Guid EnterpriseTypeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class CreateEnterpriseIndustrieDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class UpdateEnterpriseIndustrieDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
