using AbpEnterprise.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace AbpEnterprise
{
    public class EnterpriseDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<EnterpriseType, Guid> _enterpriseTypeRepository;

        public EnterpriseDataSeedContributor(IRepository<EnterpriseType, Guid> enterpriseTypeRepository)
        {
            _enterpriseTypeRepository = enterpriseTypeRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (!await _enterpriseTypeRepository.AnyAsync())
            {
                var enterpriseType1 = new EnterpriseType(
                    id: Guid.NewGuid(),
                    name: "Công ty TNHH",
                    description: "Công ty trách nhiệm hữu hạn"
                );
                enterpriseType1.AddIndustry(Guid.NewGuid(), "Công nghệ thông tin", "IT01");
                enterpriseType1.AddIndustry(Guid.NewGuid(), "Xây dựng", "CON01");
                await _enterpriseTypeRepository.InsertAsync(enterpriseType1);

                var enterpriseType2 = new EnterpriseType(
                    id: Guid.NewGuid(),
                    name: "Công ty Cổ phần",
                    description: "Công ty cổ phần với nhiều cổ đông"
                );
                enterpriseType2.AddIndustry(Guid.NewGuid(), "Sản xuất", "MAN01");
                enterpriseType2.AddIndustry(Guid.NewGuid(), "Thương mại", "TRA01");
                await _enterpriseTypeRepository.InsertAsync(enterpriseType2);

                var enterpriseType3 = new EnterpriseType(
                    id: Guid.NewGuid(),
                    name: "Doanh nghiệp tư nhân",
                    description: "Doanh nghiệp do một cá nhân làm chủ"
                );
                enterpriseType3.AddIndustry(Guid.NewGuid(), "Dịch vụ", "SER01");
                await _enterpriseTypeRepository.InsertAsync(enterpriseType3);
            }
        }
    }
}
