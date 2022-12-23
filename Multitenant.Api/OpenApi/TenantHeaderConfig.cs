using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MultiTenant.Api.OpenApi;

public class TenantHeaderConfig : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "tenant",
            In = ParameterLocation.Header,
            Style = ParameterStyle.Simple,
            Required = true
        });
    }
}