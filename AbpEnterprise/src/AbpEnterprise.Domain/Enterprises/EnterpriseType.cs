using AbpEnterprise.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace AbpEnterprise.Enterprises
{
    public class EnterpriseType : FullAuditedEntity<Guid>
    {
        public string Name { get; private set; }
        public string Code { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }

        public Guid EnterpriseIndustryId { get; private set; }

        // Navigation property
        public EnterpriseIndustry EnterpriseIndustry { get; private set; }

        protected EnterpriseType()
        {
        }

        internal EnterpriseType(
            Guid id,
            Guid enterpriseIndustryId,
            string name,
            string code,
            string description = null) : base(id)
        {
            EnterpriseIndustryId = enterpriseIndustryId;
            SetName(name);
            SetCode(code);
            SetDescription(description);
            IsActive = true;
        }


        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), EnterpriseTypeConsts.MaxNameLength);
        }

        public void SetCode(string code)
        {
            Code = Check.NotNullOrWhiteSpace(code, nameof(code), EnterpriseTypeConsts.MaxCodeLength);
        }

        public void SetDescription(string description)
        {
            Description = Check.Length(description, nameof(description), EnterpriseTypeConsts.MaxDescriptionLength);
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
