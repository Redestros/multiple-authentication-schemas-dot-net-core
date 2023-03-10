using Core.Interfaces;
using Core.Settings;
using Infrastructure.Extensions;
using Infrastructure.Services;
using Infrastructure.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MultiTenant.Api.OpenApi;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<TenantHeaderConfig>();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MultiTenant.Api", Version = "v1" });
});
builder.Services.AddTransient<ITenantService, TenantService>();
builder.Services.AddTransient<IProductService, ProductService>();

builder.Services.Configure<TenantSettings>(builder.Configuration.GetSection(nameof(TenantSettings)));
builder.Services.Configure<AuthenticationSettings>(builder.Configuration.GetSection("Authentication"));

builder.Services.AddAndMigrateTenantDatabases(builder.Configuration);

builder.Services.AddAuthenticationWithSettings();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MultiTenant.Api v1"));
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();