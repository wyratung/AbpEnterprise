using Volo.Abp.Modularity;

namespace AbpEnterprise;

public abstract class AbpEnterpriseApplicationTestBase<TStartupModule> : AbpEnterpriseTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
