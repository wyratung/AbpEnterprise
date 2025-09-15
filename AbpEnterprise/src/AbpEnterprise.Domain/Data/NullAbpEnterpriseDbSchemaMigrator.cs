using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace AbpEnterprise.Data;

/* This is used if database provider does't define
 * IAbpEnterpriseDbSchemaMigrator implementation.
 */
public class NullAbpEnterpriseDbSchemaMigrator : IAbpEnterpriseDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
