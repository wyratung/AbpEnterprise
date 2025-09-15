using Microsoft.Extensions.Localization;
using AbpEnterprise.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace AbpEnterprise;

[Dependency(ReplaceServices = true)]
public class AbpEnterpriseBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<AbpEnterpriseResource> _localizer;

    public AbpEnterpriseBrandingProvider(IStringLocalizer<AbpEnterpriseResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
