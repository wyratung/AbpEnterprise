using Volo.Abp.Modularity;

namespace AbpEnterprise;

/* Inherit from this class for your domain layer tests. */
public abstract class AbpEnterpriseDomainTestBase<TStartupModule> : AbpEnterpriseTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
