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
        public string Name { get; private set; } = null!;
        public string Code { get; private set; } = null!;
        public string? Description { get; private set; }
        public bool IsActive { get; private set; }

        public Guid EnterpriseIndustryId { get; private set; }

        private readonly List<EnterpriseTypeAddressMapping> _addressMappings = new();
        public IReadOnlyCollection<EnterpriseTypeAddressMapping> AddressMappings => _addressMappings.AsReadOnly();

        public void AddAddress(EnterpriseTypeAddress address)
        {
            Check.NotNull(address, nameof(address));
            
            if (_addressMappings.Any(x => x.EnterpriseTypeAddressId == address.Id))
            {
                return; // Address already added
            }

            _addressMappings.Add(new EnterpriseTypeAddressMapping(
                Guid.NewGuid(),
                this.Id,
                address.Id
            ));
        }

        public void RemoveAddress(Guid addressId)
        {
            var mapping = _addressMappings.FirstOrDefault(x => x.EnterpriseTypeAddressId == addressId);
            if (mapping != null)
            {
                _addressMappings.Remove(mapping);
            }
        }

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
