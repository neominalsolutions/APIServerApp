using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPIIntroduction.Dtos;

namespace WebAPIIntroduction.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TokensController : ControllerBase
  {

    // token generate işlemlerinde istekler post olarak uygulanır.
    // güvenlik gerekçesiyle POST işlemleri GET işlemlerine göre daha güvenlik bir network isteğidir.
    // hasass bilgiler username, password GET ile gönderilmez
    // /api/tokens?username=ali&password=1234
    // /api/tokens {username:"ali", password:"32324"} POST
    [HttpPost]
    public async Task<IActionResult> SignInAsync([FromBody] LoginDto model)
    {

      if(model.UserName == "ugur" && model.Password == "admin")
      {
        // kullanıcının sisteme giriş yaparken kullandığı özelliklere biz claim diyorduk
        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.Name, model.UserName));
        claims.Add(new Claim("sub", Guid.NewGuid().ToString())); // sub subject => token alan kişi yani userId
        claims.Add(new Claim(ClaimTypes.Role, "Admin"));


        var privateKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("yzl3439.fullstack.aspnet"));
        var signinCredentials = new SigningCredentials(privateKey, SecurityAlgorithms.HmacSha512);
        var tokeOptions = new JwtSecurityToken(
            issuer: "https://localhost:7091", // token üreticisinin base Url
            audience: "https://localhost:7091", // üretilen token validate den uygulama
            claims: claims,
            expires: DateTime.Now.AddMinutes(30), // token bitiş süresi 
            signingCredentials: signinCredentials // token şifreleyerek imzalama özelliği.
        );
        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokeOptions); // erişim bileti üretildi.
        return Ok(new { token = accessToken }); // kullanıcı varsa token

      }

      return Unauthorized(); // 401
    }

  }
}
