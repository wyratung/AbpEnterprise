using AbpEnterprise.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace AbpEnterprise.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class AbpEnterpriseController : AbpControllerBase
{
    protected AbpEnterpriseController()
    {
        LocalizationResource = typeof(AbpEnterpriseResource);
    }
}
