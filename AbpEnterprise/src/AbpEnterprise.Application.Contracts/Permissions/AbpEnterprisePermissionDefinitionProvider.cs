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

        var authorsPermission = myGroup.AddPermission(
    AbpEnterprisePermissions.Authors.Default, L("Permission:Authors"));
        authorsPermission.AddChild(
            AbpEnterprisePermissions.Authors.Create, L("Permission:Authors.Create"));
        authorsPermission.AddChild(
            AbpEnterprisePermissions.Authors.Edit, L("Permission:Authors.Edit"));
        authorsPermission.AddChild(
            AbpEnterprisePermissions.Authors.Delete, L("Permission:Authors.Delete"));


        // Enterprise Industries
        var industriesPermission = myGroup.AddPermission(
            AbpEnterprisePermissions.EnterpriseIndustries.Default,
            L("Permission:EnterpriseIndustries"));

        industriesPermission.AddChild(
            AbpEnterprisePermissions.EnterpriseIndustries.Create,
            L("Permission:EnterpriseIndustries.Create"));

        industriesPermission.AddChild(
            AbpEnterprisePermissions.EnterpriseIndustries.Update,
            L("Permission:EnterpriseIndustries.Update"));

        industriesPermission.AddChild(
            AbpEnterprisePermissions.EnterpriseIndustries.Delete,
            L("Permission:EnterpriseIndustries.Delete"));

        // Enterprise Types
        //var typesPermission = myGroup.AddPermission(
        //    AbpEnterprisePermissions.EnterpriseTypes.Default,
        //    L("Permission:EnterpriseTypes"));

        //typesPermission.AddChild(
        //    AbpEnterprisePermissions.EnterpriseTypes.Create,
        //    L("Permission:EnterpriseTypes.Create"));

        //typesPermission.AddChild(
        //    AbpEnterprisePermissions.EnterpriseTypes.Update,
        //    L("Permission:EnterpriseTypes.Update"));

        //typesPermission.AddChild(
        //    AbpEnterprisePermissions.EnterpriseTypes.Delete,
        //    L("Permission:EnterpriseTypes.Delete"));

        // Enterprise Type Addresses
        var enterpriseTypeAddressesPermission = myGroup.AddPermission(
            AbpEnterprisePermissions.EnterpriseTypeAddresses.Default,
            L("Permission:EnterpriseTypeAddresses"));
        enterpriseTypeAddressesPermission.AddChild(
            AbpEnterprisePermissions.EnterpriseTypeAddresses.Create,
            L("Permission:EnterpriseTypeAddresses.Create"));
        enterpriseTypeAddressesPermission.AddChild(
            AbpEnterprisePermissions.EnterpriseTypeAddresses.Edit,
            L("Permission:EnterpriseTypeAddresses.Edit"));
        enterpriseTypeAddressesPermission.AddChild(
            AbpEnterprisePermissions.EnterpriseTypeAddresses.Delete,
            L("Permission:EnterpriseTypeAddresses.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpEnterpriseResource>(name);
    }
}
