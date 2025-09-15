using Volo.Abp.Settings;

namespace AbpEnterprise.Settings;

public class AbpEnterpriseSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(AbpEnterpriseSettings.MySetting1));
    }
}
