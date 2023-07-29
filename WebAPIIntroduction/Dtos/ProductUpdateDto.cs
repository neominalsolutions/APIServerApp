using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAPIIntroduction.Dtos
{
  public class ProductUpdateDto
  {
    [JsonPropertyName("unitStock")]
    [Required(ErrorMessage = "Stok alanı boş geçilemez")]
    [Range(10,100, ErrorMessage = "Min 10 Maksimum 100 stok alınabilir")]
    public short? Stock { get; set; }

    [JsonPropertyName("unitPrice")]
    [Min(0, ErrorMessage = "Ürün fiyatı 0 dan düşük olamaz")]
    [Required(ErrorMessage = "Fiyat alanı boş geçilemez")]
    public decimal? Price { get; set; }
  }
}
