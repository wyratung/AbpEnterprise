using AbpEnterprise.Samples;
using Xunit;

namespace AbpEnterprise.EntityFrameworkCore.Domains;

[Collection(AbpEnterpriseTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<AbpEnterpriseEntityFrameworkCoreTestModule>
{

}
