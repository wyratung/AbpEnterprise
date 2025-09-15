using Volo.Abp.Modularity;

namespace AbpEnterprise;

[DependsOn(
    typeof(AbpEnterpriseApplicationModule),
    typeof(AbpEnterpriseDomainTestModule)
)]
public class AbpEnterpriseApplicationTestModule : AbpModule
{

}
