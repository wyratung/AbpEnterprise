using AbpEnterprise.Enterpries;
using AbpEnterprise.Enterpries.EnterpriseInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace AbpEnterprise.Enterprises.DomainServices
{
    public class EnterpriseIndustryManager : DomainService
    {
        private readonly IEnterpriseIndustryRepository _enterpriseIndustryRepository;
        private readonly IEnterpriseTypeRepository _enterpriseTypeRepository;

        public EnterpriseIndustryManager(
            IEnterpriseIndustryRepository enterpriseIndustryRepository,
            IEnterpriseTypeRepository enterpriseTypeRepository)
        {
            _enterpriseIndustryRepository = enterpriseIndustryRepository;
            _enterpriseTypeRepository = enterpriseTypeRepository;
        }

        public async Task<EnterpriseIndustry> CreateAsync(
            string name,
            string code,
            string description = null
            )
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNullOrWhiteSpace(code, nameof(code));

            await CheckIndustryCodeUniquenessAsync(code);

            return new EnterpriseIndustry(
                GuidGenerator.Create(),
                name,
                code,
                description                
            );
        }

        public async Task<EnterpriseType> CreateEnterpriseTypeAsync(
            EnterpriseIndustry industry,
            string name,
            string code,
            string description = null)
        {
            Check.NotNull(industry, nameof(industry));
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNullOrWhiteSpace(code, nameof(code));

            await CheckEnterpriseTypeCodeUniquenessAsync(industry.Id, code);

            var enterpriseType = new EnterpriseType(
                GuidGenerator.Create(),
                industry.Id,
                name,
                code,
                description
            );

            industry.AddEnterpriseType(enterpriseType);
            return enterpriseType;
        }

        public async Task ChangeCodeAsync(EnterpriseIndustry industry, string code)
        {
            Check.NotNull(industry, nameof(industry));
            Check.NotNullOrWhiteSpace(code, nameof(code));

            if (industry.Code == code)
                return;

            await CheckIndustryCodeUniquenessAsync(code, industry.Id);
            industry.SetCode(code);
        }

        private async Task CheckIndustryCodeUniquenessAsync(string code, Guid? excludeId = null)
        {
            var exists = await _enterpriseIndustryRepository
                .AnyAsync(ei => ei.Code == code && ei.Id != excludeId);

            if (exists)
            {
                throw new BusinessException(EnterpriseDomainErrorCodes.EnterpriseIndustryCodeAlreadyExists)
                    .WithData("Code", code);
            }
        }

        private async Task CheckEnterpriseTypeCodeUniquenessAsync(Guid industryId, string code, Guid? excludeId = null)
        {
            var exists = await _enterpriseTypeRepository
                .AnyAsync(et => et.EnterpriseIndustryId == industryId && et.Code == code && et.Id != excludeId);

            if (exists)
            {
                throw new BusinessException(EnterpriseDomainErrorCodes.EnterpriseTypeCodeAlreadyExists)
                    .WithData("Code", code);
            }
        }
    }
}
