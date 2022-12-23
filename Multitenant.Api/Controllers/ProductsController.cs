using System.Threading.Tasks;
using Core.Interfaces;
using Core.Settings;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MultiTenant.Api.Controllers;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = AuthSchemas.FrontSchema)]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(int id)
    {
        var productDetails = await _service.GetByIdAsync(id);
        return Ok(productDetails);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateProductRequest request)
    {
        return Ok(await _service.CreateAsync(request.Name, request.Description, request.Rate));
    }
}