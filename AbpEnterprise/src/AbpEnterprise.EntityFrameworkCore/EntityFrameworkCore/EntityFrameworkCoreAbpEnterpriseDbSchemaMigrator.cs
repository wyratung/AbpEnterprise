using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AbpEnterprise.Data;
using Volo.Abp.DependencyInjection;

namespace AbpEnterprise.EntityFrameworkCore;

public class EntityFrameworkCoreAbpEnterpriseDbSchemaMigrator
    : IAbpEnterpriseDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreAbpEnterpriseDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the AbpEnterpriseDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<AbpEnterpriseDbContext>()
            .Database
            .MigrateAsync();
    }
}
