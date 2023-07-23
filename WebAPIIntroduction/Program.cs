var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Cross Origin Resource Sharing
// farkl� lokasyonda yay�nlanan uygulamalar aras� kaynak payla��m�n� ayarla
builder.Services.AddCors(opt => 
opt.AddDefaultPolicy(policy =>
{
  policy.AllowAnyOrigin(); // t�m site isteklerini kabul et
  policy.AllowAnyMethod();  // t�m method isteklerini kabul et, HTTPGET,HTTPPOSt,HTTPDELETE,HTTPPUT
  policy.AllowAnyHeader(); // t�m headerlar� kabul et. client uygulama sunucuya baz� de�erleri headerdan g�nderir.

}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseCors(); // ara yaz�l�m ile cors ayarlar�n� uygula

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
