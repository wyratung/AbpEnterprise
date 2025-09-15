using AbpEnterprise.Books;
using Xunit;

namespace AbpEnterprise.EntityFrameworkCore.Applications.Books;

[Collection(AbpEnterpriseTestConsts.CollectionDefinitionName)]
public class EfCoreBookAppService_Tests : BookAppService_Tests<AbpEnterpriseEntityFrameworkCoreTestModule>
{

}