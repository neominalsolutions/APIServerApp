using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAPIIntroduction.Dtos
{
  public class ChangeProductNameDto
  {
    [JsonPropertyName("productName")]
    [Required(ErrorMessage = "Ürün ismi boş geçilemez")]
    public string? Name { get; set; }

  }
}
