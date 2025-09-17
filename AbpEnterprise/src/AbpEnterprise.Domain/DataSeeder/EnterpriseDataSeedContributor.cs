using AbpEnterprise.Enterpries.EnterpriseInterfaces;
using AbpEnterprise.Enterprises;
using AbpEnterprise.Enterprises.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace AbpEnterprise.DataSeeder
{
    public class EnterpriseDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IEnterpriseIndustryRepository _enterpriseIndustryRepository;
        private readonly EnterpriseIndustryManager _enterpriseIndustryManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public EnterpriseDataSeedContributor(
            IEnterpriseIndustryRepository enterpriseIndustryRepository,
            EnterpriseIndustryManager enterpriseIndustryManager,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _enterpriseIndustryRepository = enterpriseIndustryRepository;
            _enterpriseIndustryManager = enterpriseIndustryManager;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            using var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true);

            await SeedEnterpriseIndustriesAsync();

            await uow.CompleteAsync();
        }

        private async Task SeedEnterpriseIndustriesAsync()
        {
            if (await _enterpriseIndustryRepository.GetCountAsync() > 0)
                return;

            // Seed Industries
            var industries = new[]
            {
                new { Name = "Công nghệ thông tin", Code = "IT", Description = "Ngành công nghệ thông tin và truyền thông", SortOrder = 1 },
                new { Name = "Sản xuất", Code = "MFG", Description = "Ngành sản xuất và chế tạo", SortOrder = 2 },
                new { Name = "Dịch vụ", Code = "SVC", Description = "Ngành dịch vụ", SortOrder = 3 },
                new { Name = "Thương mại", Code = "COM", Description = "Ngành thương mại và bán lẻ", SortOrder = 4 }
            };

            foreach (var industryData in industries)
            {
                var industry = await _enterpriseIndustryManager.CreateAsync(
                    industryData.Name,
                    industryData.Code,
                    industryData.Description);

                await _enterpriseIndustryRepository.InsertAsync(industry);

                // Seed Enterprise Types for each industry
                await SeedEnterpriseTypesAsync(industry);
            }
        }

        private async Task SeedEnterpriseTypesAsync(EnterpriseIndustry industry)
        {
            var typesData = industry.Code switch
            {
                "IT" => new[]
                {
                    new { Name = "Phần mềm", Code = "SW", Description = "Công ty phần mềm", SortOrder = 1 },
                    new { Name = "Phần cứng", Code = "HW", Description = "Công ty phần cứng", SortOrder = 2 },
                    new { Name = "Dịch vụ IT", Code = "ITS", Description = "Dịch vụ công nghệ thông tin", SortOrder = 3 }
                },
                "MFG" => new[]
                {
                    new { Name = "Sản xuất cơ khí", Code = "MECH", Description = "Sản xuất cơ khí", SortOrder = 1 },
                    new { Name = "Sản xuất điện tử", Code = "ELEC", Description = "Sản xuất điện tử", SortOrder = 2 },
                    new { Name = "Sản xuất hóa chất", Code = "CHEM", Description = "Sản xuất hóa chất", SortOrder = 3 }
                },
                "SVC" => new[]
                {
                    new { Name = "Tư vấn", Code = "CONS", Description = "Dịch vụ tư vấn", SortOrder = 1 },
                    new { Name = "Giáo dục", Code = "EDU", Description = "Dịch vụ giáo dục", SortOrder = 2 },
                    new { Name = "Y tế", Code = "MED", Description = "Dịch vụ y tế", SortOrder = 3 }
                },
                "COM" => new[]
                {
                    new { Name = "Bán lẻ", Code = "RET", Description = "Bán lẻ", SortOrder = 1 },
                    new { Name = "Bán sỉ", Code = "WHO", Description = "Bán sỉ", SortOrder = 2 },
                    new { Name = "Xuất nhập khẩu", Code = "IMP", Description = "Xuất nhập khẩu", SortOrder = 3 }
                },
                _ => Array.Empty<dynamic>()
            };

            foreach (var typeData in typesData)
            {
                await _enterpriseIndustryManager.CreateEnterpriseTypeAsync(
                    industry,
                    typeData.Name,
                    typeData.Code,
                    typeData.Description);
            }
        }
    }
}
