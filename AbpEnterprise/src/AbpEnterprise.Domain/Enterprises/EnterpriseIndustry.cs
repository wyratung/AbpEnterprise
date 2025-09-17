using AbpEnterprise.Enterpries;
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
    public class EnterpriseIndustry : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; private set; }
        public string Code { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }

        private readonly List<EnterpriseType> _enterpriseTypes = new();
        public IReadOnlyList<EnterpriseType> EnterpriseTypes => _enterpriseTypes.AsReadOnly();

        protected EnterpriseIndustry()
        {
        }

        // Constructor cho việc tạo mới
        internal EnterpriseIndustry(
            Guid id,
            string name,
            string code,
            string description = null) : base(id)
        {
            SetName(name);
            SetCode(code);
            SetDescription(description);
            IsActive = true;
        }

        // Business methods
        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), EnterpriseIndustryConsts.MaxNameLength);
        }

        public void SetCode(string code)
        {
            Code = Check.NotNullOrWhiteSpace(code, nameof(code), EnterpriseIndustryConsts.MaxCodeLength);
        }

        public void SetDescription(string description)
        {
            Description = Check.Length(description, nameof(description), EnterpriseIndustryConsts.MaxDescriptionLength);
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            if (_enterpriseTypes.Any(et => et.IsActive))
            {
                throw new BusinessException(EnterpriseDomainErrorCodes.CannotDeactivateIndustryWithActiveTypes);
            }
            IsActive = false;
        }

        public void AddEnterpriseType(EnterpriseType enterpriseType)
        {
            Check.NotNull(enterpriseType, nameof(enterpriseType));

            if (_enterpriseTypes.Any(et => et.Code == enterpriseType.Code))
            {
                throw new BusinessException(EnterpriseDomainErrorCodes.EnterpriseTypeCodeAlreadyExists);
            }

            _enterpriseTypes.Add(enterpriseType);
        }

        public void RemoveEnterpriseType(Guid enterpriseTypeId)
        {
            var enterpriseType = _enterpriseTypes.FirstOrDefault(et => et.Id == enterpriseTypeId);
            if (enterpriseType != null)
            {
                _enterpriseTypes.Remove(enterpriseType);
            }
        }
    }
}
