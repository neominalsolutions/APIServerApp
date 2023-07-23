using System.Text.Json.Serialization;

namespace WebAPIIntroduction.Dtos
{
  public class ProductCreateDto
  {
    [JsonPropertyName("productName")]
    public string Name { get; set; }

    [JsonPropertyName("unitPrice")]
    public decimal Price { get; set; }


  }
}
