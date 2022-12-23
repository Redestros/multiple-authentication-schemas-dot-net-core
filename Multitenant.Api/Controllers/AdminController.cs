using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MultiTenant.Api.Controllers;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = AuthSchemas.AdministrationSchema)]
[ApiController]
public class AdminController : ControllerBase
{

    [HttpGet]
    public IActionResult SayHi()
    {
        return Ok("Hello admin");
    }
}