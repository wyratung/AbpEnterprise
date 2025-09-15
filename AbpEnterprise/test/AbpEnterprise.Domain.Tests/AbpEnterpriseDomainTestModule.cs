using Volo.Abp.Modularity;

namespace AbpEnterprise;

[DependsOn(
    typeof(AbpEnterpriseDomainModule),
    typeof(AbpEnterpriseTestBaseModule)
)]
public class AbpEnterpriseDomainTestModule : AbpModule
{

}
