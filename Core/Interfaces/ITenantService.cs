using Core.Settings;

namespace Core.Interfaces;

public interface ITenantService
{
    public string GetDatabaseProvider();

    public string GetConnectionString();

    public Tenant GetTenant();
}