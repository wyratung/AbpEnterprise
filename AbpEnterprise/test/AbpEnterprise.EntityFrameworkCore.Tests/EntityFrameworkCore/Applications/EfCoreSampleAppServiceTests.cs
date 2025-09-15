using AbpEnterprise.Samples;
using Xunit;

namespace AbpEnterprise.EntityFrameworkCore.Applications;

[Collection(AbpEnterpriseTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<AbpEnterpriseEntityFrameworkCoreTestModule>
{

}
