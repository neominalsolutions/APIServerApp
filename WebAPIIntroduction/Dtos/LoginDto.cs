using System.Text.Json.Serialization;

namespace WebAPIIntroduction.Dtos
{
  // Data Transfer Object
  // farklı platformlar üzerinden veri entegrasyonunda kullanılan bir obje formatı.
  public class LoginDto
  {
    [JsonPropertyName("username")]
    public string UserName { get; set; }

    [JsonPropertyName("pass")]
    public string Password { get; set; }

  }
}
