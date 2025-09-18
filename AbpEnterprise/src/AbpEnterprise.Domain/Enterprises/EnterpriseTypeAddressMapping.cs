using AbpEnterprise.Enterpries.Enum;
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace AbpEnterprise.Enterprises
{
    public class EnterpriseTypeAddressMapping : FullAuditedEntity<Guid>
    {
        public Guid EnterpriseTypeId { get; private set; }

        public Guid EnterpriseTypeAddressId { get; private set; }

        // Navigation properties (optional for queries)
        public EnterpriseType EnterpriseType { get; private set; }
        public EnterpriseTypeAddress EnterpriseTypeAddress { get; private set; }

        private EnterpriseTypeAddressMapping() { }

        public EnterpriseTypeAddressMapping(
            Guid id,
            Guid enterpriseTypeId,
            Guid enterpriseTypeAddressId
            ) : base(id)
        {
            EnterpriseTypeId = enterpriseTypeId;
            EnterpriseTypeAddressId = enterpriseTypeAddressId;    
        }  
    }
}