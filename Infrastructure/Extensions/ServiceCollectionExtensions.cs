using System.Linq;
using Core.Settings;
using Infrastructure.Persistence;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAndMigrateTenantDatabases(this IServiceCollection services,
        IConfiguration config)
    {
        var options = services.GetOptions<TenantSettings>(nameof(TenantSettings));
        var defaultConnectionString = options.Defaults?.ConnectionString;
        var defaultDbProvider = options.Defaults?.DbProvider;
        if (defaultDbProvider != null && defaultDbProvider.ToLower() == "mssql")
            services.AddDbContext<ApplicationDbContext>(m =>
                m.UseSqlServer(e => e.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        var tenants = options.Tenants;
        foreach (var connectionString in tenants.Select(tenant =>
                     string.IsNullOrEmpty(tenant.ConnectionString) ? defaultConnectionString : tenant.ConnectionString))
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.SetConnectionString(connectionString);
            if (dbContext.Database.GetMigrations().Any()) dbContext.Database.Migrate();
        }

        return services;
    }

    public static IServiceCollection AddAuthenticationWithSettings(this IServiceCollection services)
    {
        var authOptions = services.GetOptions<AuthenticationSettings>("Authentication");

        var clientSettings = authOptions.Front;
        var administrationSettings = authOptions.Administration;

        services.AddAuthentication(AuthSchemas.FrontSchema)
            .AddJwtBearer(AuthSchemas.FrontSchema, options =>
            {
                options.Authority = clientSettings.Authority;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidAudiences = clientSettings.ValidAudiences,
                    ValidIssuers = clientSettings.ValidIssuers
                };
            })
            .AddJwtBearer(AuthSchemas.AdministrationSchema, options =>
            {
                options.Authority = administrationSettings.Authority;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidAudiences = administrationSettings.ValidAudiences,
                    ValidIssuers = administrationSettings.ValidIssuers
                };
            });
        return services;
    }

    private static T GetOptions<T>(this IServiceCollection services, string sectionName) where T : new()
    {
        using var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var section = configuration.GetSection(sectionName);
        var options = new T();
        section.Bind(options);
        return options;
    }
}