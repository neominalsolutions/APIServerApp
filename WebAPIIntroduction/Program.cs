using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
  opt.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
  {
    Name = "Authorization",
    Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer"
  });

  opt.AddSecurityRequirement(new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme
        {
            Name = "Bearer",
            In = ParameterLocation.Header,
            Reference = new OpenApiReference
            {
                Id = "Bearer",
                Type = ReferenceType.SecurityScheme
            }
        },
        new List<string>()
    }
});
});
// Cross Origin Resource Sharing
// farkl� lokasyonda yay�nlanan uygulamalar aras� kaynak payla��m�n� ayarla
builder.Services.AddCors(opt => 

opt.AddDefaultPolicy(policy =>
{
  policy.AllowAnyOrigin(); // t�m site isteklerini kabul et
  policy.AllowAnyMethod();  // t�m method isteklerini kabul et, HTTPGET,HTTPPOSt,HTTPDELETE,HTTPPUT
  policy.AllowAnyHeader(); // t�m headerlar� kabul et. client uygulama sunucuya baz� de�erleri headerdan g�nderir.

}));

string privateKey = "yzl3439.fullstack.aspnet"; // ascii karakterler �zerinden byte dizisine d�n��tr�r�r.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,opt =>
{
  opt.SaveToken = true; // save token �retilen accesstoken bilgisine controller actionlardan ula��labiliyor.
  opt.MapInboundClaims = true; // Authenticate olan ki�ini Name ve Role alar�n� uygulama i�erisinde �ekebilmek eri�ebilmek i�in bir ayar. User.Identity.Name k�sm�n�nda login olan kullan�c� bilgileri okunabilir.

  opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
  {
    ValidateAudience = false,
    ValidateIssuer = false,
    ValidateLifetime = true, // token expire olana kadar validate do�rulans�n diye yap�lan bir kontrol �zelli�i. sistem token s�resi dolunca token kabul etmesin
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey)), // private key de�erine g�re tokenm�z� validate etmek i�in kulland���m�z bir do�rulama
  };


});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseCors(); // ara yaz�l�m ile cors ayarlar�n� uygula

app.UseHttpsRedirection();

app.UseAuthentication(); // JWT ile kimlik do�rulamay� aktif hale getir.
app.UseAuthorization();

app.MapControllers();

app.Run();
