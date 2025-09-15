﻿using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AbpEnterprise.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class AbpEnterpriseDbContextFactory : IDesignTimeDbContextFactory<AbpEnterpriseDbContext>
{
    public AbpEnterpriseDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        AbpEnterpriseEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<AbpEnterpriseDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));
        
        return new AbpEnterpriseDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../AbpEnterprise.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
