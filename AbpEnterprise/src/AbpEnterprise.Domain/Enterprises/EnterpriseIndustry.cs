using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace AbpEnterprise.Enterprises
{
    public class EnterpriseIndustry : Entity<Guid>
    {
        public Guid EnterpriseTypeId { get; private set; } 
        public string IndustryName { get; private set; } 
        public string IndustryCode { get; private set; } 

        protected EnterpriseIndustry() { }

        public EnterpriseIndustry(Guid id, Guid enterpriseTypeId, string industryName, string industryCode) : base(id)
        {
            EnterpriseTypeId = enterpriseTypeId;
            IndustryName = industryName ?? throw new ArgumentNullException(nameof(industryName));
            IndustryCode = industryCode ?? throw new ArgumentNullException(nameof(industryCode));
        }

        public void Update(string industryName, string industryCode)
        {
            IndustryName = industryName ?? throw new ArgumentNullException(nameof(industryName));
            IndustryCode = industryCode ?? throw new ArgumentNullException(nameof(industryCode));
        }
    }
}
