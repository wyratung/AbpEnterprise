using Volo.Abp.Account;
using Volo.Abp.Auditing;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace AbpEnterprise;

[DependsOn(
    typeof(AbpEnterpriseDomainModule),
    typeof(AbpEnterpriseApplicationContractsModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpAccountApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule)
    )]
public class AbpEnterpriseApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<AbpEnterpriseApplicationModule>();
        });
        Configure<AbpAuditingOptions>(options =>
        {
            options.IsEnabled = true;                  
            options.IsEnabledForGetRequests = true;    
            options.AlwaysLogOnException = true;       
        });
    }
}
