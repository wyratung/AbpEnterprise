using AbpEnterprise.Localization;
using Volo.Abp.Application.Services;

namespace AbpEnterprise;

/* Inherit your application services from this class.
 */
public abstract class AbpEnterpriseAppService : ApplicationService
{
    protected AbpEnterpriseAppService()
    {
        LocalizationResource = typeof(AbpEnterpriseResource);
    }
}
