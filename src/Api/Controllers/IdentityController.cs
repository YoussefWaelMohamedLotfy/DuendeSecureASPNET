using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public sealed class IdentityController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
        => Ok(from c in User.Claims select new { c.Type, c.Value });
}