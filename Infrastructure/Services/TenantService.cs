using System;
using System.Linq;
using Core.Interfaces;
using Core.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class TenantService : ITenantService
{
    private readonly TenantSettings _tenantSettings;
    private Tenant _currentTenant;

    public TenantService(IOptions<TenantSettings> tenantSettings, IHttpContextAccessor contextAccessor)
    {
        _tenantSettings = tenantSettings.Value;
        var httpContext = contextAccessor.HttpContext;
        if (httpContext == null) return;
        if (httpContext.Request.Headers.TryGetValue("tenant", out var tenantId))
            SetTenant(tenantId);
        else
            throw new Exception("Invalid Tenant!");
    }

    public string GetConnectionString()
    {
        return _currentTenant?.ConnectionString;
    }

    public string GetDatabaseProvider()
    {
        return _tenantSettings.Defaults?.DbProvider;
    }

    public Tenant GetTenant()
    {
        return _currentTenant;
    }

    private void SetTenant(string tenantId)
    {
        _currentTenant = _tenantSettings.Tenants.FirstOrDefault(a => a.Tid == tenantId);
        if (_currentTenant == null) throw new Exception("Invalid Tenant!");
        if (string.IsNullOrEmpty(_currentTenant.ConnectionString)) SetDefaultConnectionStringToCurrentTenant();
    }

    private void SetDefaultConnectionStringToCurrentTenant()
    {
        _currentTenant.ConnectionString = _tenantSettings.Defaults.ConnectionString;
    }
}