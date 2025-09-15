using AbpEnterprise.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AbpEnterprise.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpEnterpriseEntityFrameworkCoreModule),
    typeof(AbpEnterpriseApplicationContractsModule)
)]
public class AbpEnterpriseDbMigratorModule : AbpModule
{
}
