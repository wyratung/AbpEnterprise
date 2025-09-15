using AbpEnterprise.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace AbpEnterprise.Permissions;

public class AbpEnterprisePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(AbpEnterprisePermissions.GroupName);

        var booksPermission = myGroup.AddPermission(AbpEnterprisePermissions.Books.Default, L("Permission:Books"));
        booksPermission.AddChild(AbpEnterprisePermissions.Books.Create, L("Permission:Books.Create"));
        booksPermission.AddChild(AbpEnterprisePermissions.Books.Edit, L("Permission:Books.Edit"));
        booksPermission.AddChild(AbpEnterprisePermissions.Books.Delete, L("Permission:Books.Delete"));
        //Define your own permissions here. Example:
        //myGroup.AddPermission(AbpEnterprisePermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpEnterpriseResource>(name);
    }
}
