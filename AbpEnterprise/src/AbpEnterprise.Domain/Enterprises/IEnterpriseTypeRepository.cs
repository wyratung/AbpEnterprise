using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace AbpEnterprise.Enterprises
{
    public interface IEnterpriseTypeRepository : IRepository<EnterpriseType, Guid>
    {
        Task<EnterpriseType> GetWithIndustriesAsync(Guid id);
        Task<EnterpriseType> FindByNameAsync(string name);
    }    
}
