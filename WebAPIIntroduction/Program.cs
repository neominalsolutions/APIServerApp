var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Cross Origin Resource Sharing
// farklý lokasyonda yayýnlanan uygulamalar arasý kaynak paylaþýmýný ayarla
builder.Services.AddCors(opt => 
opt.AddDefaultPolicy(policy =>
{
  policy.AllowAnyOrigin(); // tüm site isteklerini kabul et
  policy.AllowAnyMethod();  // tüm method isteklerini kabul et, HTTPGET,HTTPPOSt,HTTPDELETE,HTTPPUT
  policy.AllowAnyHeader(); // tüm headerlarý kabul et. client uygulama sunucuya bazý deðerleri headerdan gönderir.

}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseCors(); // ara yazýlým ile cors ayarlarýný uygula

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
