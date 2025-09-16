using AbpEnterprise.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AbpEnterprise.Books
{
    [Collection(AbpEnterpriseTestConsts.CollectionDefinitionName)]
    public class EfCoreAuthorAppService_Tests : AuthorAppService_Tests<AbpEnterpriseEntityFrameworkCoreTestModule>
    {

    }
}
