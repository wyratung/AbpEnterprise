using Xunit;

namespace AbpEnterprise.EntityFrameworkCore;

[CollectionDefinition(AbpEnterpriseTestConsts.CollectionDefinitionName)]
public class AbpEnterpriseEntityFrameworkCoreCollection : ICollectionFixture<AbpEnterpriseEntityFrameworkCoreFixture>
{

}
