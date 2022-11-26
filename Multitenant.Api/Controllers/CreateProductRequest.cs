namespace MultiTenant.Api.Controllers;

public class CreateProductRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Rate { get; set; }
}