namespace AbpEnterprise.Permissions;

public static class AbpEnterprisePermissions
{
    public const string GroupName = "AbpEnterprise";


    public static class Books
    {
        public const string Default = GroupName + ".Books";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
    public static class Authors
    {
        public const string Default = GroupName + ".Authors";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";
    public static class EnterpriseIndustries
    {
        public const string Default = GroupName + ".EnterpriseIndustries";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    //public static class EnterpriseTypes
    //{
    //    public const string Default = GroupName + ".EnterpriseTypes";
    //    public const string Create = Default + ".Create";
    //    public const string Update = Default + ".Update";
    //    public const string Delete = Default + ".Delete";
    //}
}
