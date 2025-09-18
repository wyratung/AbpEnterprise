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
        public EnterpriseType GetEnterpriseTypeByCode(string code)
        {
            Check.NotNullOrWhiteSpace(code, nameof(code));
            return _enterpriseTypes.FirstOrDefault(x => x.Code == code);
        }

        public bool HasEnterpriseType(Guid typeId)
        {
            return _enterpriseTypes.Any(x => x.Id == typeId);
        }
        public EnterpriseType GetEnterpriseType(Guid typeId)
        {
            if(HasEnterpriseType(typeId))
            {
                return _enterpriseTypes.FirstOrDefault(x => x.Id == typeId);
            }
            return null;
        }

        public bool HasActiveEnterpriseTypes()
        {
            return _enterpriseTypes.Any(x => x.IsActive);
        }

        public void UpdateEnterpriseType(Guid typeId, string name, string description = null, int? sortOrder = null)
        {
            var enterpriseType = GetEnterpriseType(typeId);
            if (enterpriseType == null)
            {
                throw new BusinessException(EnterpriseDomainErrorCodes.EnterpriseTypeNotFound);
            }

            enterpriseType.SetName(name);
            if (description != null)
                enterpriseType.SetDescription(description);
           
        }

        

        public List<EnterpriseType> GetActiveEnterpriseTypes()
        {
            return _enterpriseTypes.Where(x => x.IsActive).OrderBy(x => x.Name).ToList();
        }

        public int GetEnterpriseTypesCount()
        {
            return _enterpriseTypes.Count;
        }

        public int GetActiveEnterpriseTypesCount()
        {
            return _enterpriseTypes.Count(x => x.IsActive);
        }
    }
}
