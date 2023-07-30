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
// farklý lokasyonda yayýnlanan uygulamalar arasý kaynak paylaþýmýný ayarla
builder.Services.AddCors(opt => 

opt.AddDefaultPolicy(policy =>
{
  policy.AllowAnyOrigin(); // tüm site isteklerini kabul et
  policy.AllowAnyMethod();  // tüm method isteklerini kabul et, HTTPGET,HTTPPOSt,HTTPDELETE,HTTPPUT
  policy.AllowAnyHeader(); // tüm headerlarý kabul et. client uygulama sunucuya bazý deðerleri headerdan gönderir.

}));

string privateKey = "yzl3439.fullstack.aspnet"; // ascii karakterler üzerinden byte dizisine dönüþtrürür.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,opt =>
{
  opt.SaveToken = true; // save token üretilen accesstoken bilgisine controller actionlardan ulaþýlabiliyor.
  opt.MapInboundClaims = true; // Authenticate olan kiþini Name ve Role alarýný uygulama içerisinde çekebilmek eriþebilmek için bir ayar. User.Identity.Name kýsmýnýnda login olan kullanýcý bilgileri okunabilir.

  opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
  {
    ValidateAudience = false,
    ValidateIssuer = false,
    ValidateLifetime = true, // token expire olana kadar validate doðrulansýn diye yapýlan bir kontrol özelliði. sistem token süresi dolunca token kabul etmesin
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey)), // private key deðerine göre tokenmýzý validate etmek için kullandýðýmýz bir doðrulama
  };


});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseCors(); // ara yazýlým ile cors ayarlarýný uygula

app.UseHttpsRedirection();

app.UseAuthentication(); // JWT ile kimlik doðrulamayý aktif hale getir.
app.UseAuthorization();

app.MapControllers();

app.Run();
