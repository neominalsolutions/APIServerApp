using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPIIntroduction.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ResourcesController : ControllerBase
  {
    // aşağıdaki her iki endpointe token göndermeden erişimeyeceğiz. 401 hatası alacağız.

    [HttpGet("onlyAuthenticatedUser")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // JWT ile authorize et
    public IActionResult AuthenticatedUser()
    {
      return Ok("Only authenticated User"); // sadece Token olan görebilir
    }

    // sadece rolü admin olabilenler görebilsin

    [HttpGet("onlyAdmin")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Admin")] // eğer token varsa admin değilsek 403 Access Denied hatası alırız. Eğer token yoksa 401 UnAuthorized hatası alırız.
    public IActionResult OnlySeeAdmin()
    {
      return Ok("Only admin");
    }
  }
}
