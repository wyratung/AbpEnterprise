using AbpEnterprise.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace AbpEnterprise.Enterprises
{
    public class EnterpriseType : AggregateRoot<Guid>
    {
        public string Name { get; private set; } 
        public string Description { get; private set; } 
        private readonly List<EnterpriseIndustry> _industries = new(); 
        public IReadOnlyCollection<EnterpriseIndustry> Industries => _industries.AsReadOnly();

        protected EnterpriseType()
        {
            _industries = new List<EnterpriseIndustry>();
        }

        public EnterpriseType(Guid id, string name, string description) : base(id)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            _industries = new List<EnterpriseIndustry>();
        }

        public void AddIndustry(Guid industryId, string industryName, string industryCode)
        {
            if (_industries.Any(i => i.IndustryCode == industryCode))
            {
                throw new Exception($"Industry with code {industryCode} already exists in this EnterpriseType.");
            }

            _industries.Add(new EnterpriseIndustry(industryId, this.Id, industryName, industryCode));
        }

        public void RemoveIndustry(Guid industryId)
        {
            var industry = _industries.FirstOrDefault(i => i.Id == industryId);
            if (industry == null)
            {
                throw new Exception($"Industry with ID {industryId} not found.");
            }

            _industries.Remove(industry);
        }

        public void Update(string name, string description)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
        }
    }
}
