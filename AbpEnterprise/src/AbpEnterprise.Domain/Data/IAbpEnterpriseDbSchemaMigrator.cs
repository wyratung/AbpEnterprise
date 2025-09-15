using System.Threading.Tasks;

namespace AbpEnterprise.Data;

public interface IAbpEnterpriseDbSchemaMigrator
{
    Task MigrateAsync();
}
