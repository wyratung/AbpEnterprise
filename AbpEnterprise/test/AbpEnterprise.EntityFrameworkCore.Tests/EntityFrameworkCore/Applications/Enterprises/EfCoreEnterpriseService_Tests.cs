using AbpEnterprise.Books;
using AbpEnterprise.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AbpEnterprise.EntityFrameworkCore.Applications.Enterprises
{
    [Collection(AbpEnterpriseTestConsts.CollectionDefinitionName)]
    public class EfCoreEnterpriseService_Tests : EnterpriseIndustryService_Tests<AbpEnterpriseEntityFrameworkCoreTestModule>
    {

    }
}
