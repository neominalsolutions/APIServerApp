using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIIntroduction.Data;
using WebAPIIntroduction.Data.Entities;
using WebAPIIntroduction.Dtos;

namespace WebAPIIntroduction.Controllers
{
  [Route("api/[controller]")] // Url formatımız bu
  [ApiController]
  public class ProductsController : ControllerBase
  {
    // api standartları gereki
    // 1. Controller isimler s takısı ile biter
    // 2. Create işlemleri başarılı ise 201 döner
    // 3. Get istekleri başarılı ise 200 döner
    // 4. Put ve Delete işlemleri başarılı ise 204 döner
    // 5. Validasyon hataları 400 döner
    // 6. Uygulamada bir hata meydana gelirse 500 döner
    // 7. Uygulamada kaynak bulunamazsa 404 döner
    // 8. Action yetkimiz yoksa oturum açmadıksak 401 döner
    // 9. Oturum açtıysak fakat yetkimiz yoksa 403 döner
    // 10. Url formatı Controller/action şeklide değildir.
    // 11. Url formatı Controller şeklinde oluşur.
    // api/students GET,      (Listele)
    // api/students POST,     (Kaydet)
    // api/students/3 DELETE, (Sil)
    // api/students/4 PUT (Güncelle)
    // api/students/1 GET (Detay)

    [HttpGet]
    public IActionResult Get()
    {
      var db = new AppDbContext();
      var plist = db.Products.ToList();
      // get isteklerinden OK statuscode 200 result döndürüyoruz.
      return Ok(plist);
    }


    [HttpPost]
    public IActionResult Create([FromBody] ProductCreateDto model)
    {
      var p = new Product();
      p.Name = model.Name;
      p.Price = model.Price;

      var db = new AppDbContext();
      db.Products.Add(p);

      db.SaveChanges();

      // OK diyince json tipinde result dönücek.
      return Created($"api/Products/{p.Id}", model); // 201
    }

    // api/Products/1
    [HttpGet("{id:int}")] // dışardan dinamik olarak id değeri alıcaz.
    // {id} gibi süsülü parantez içinde tanımlanan değerlere farklı değerler gelebilir. 1,2,3,5
    public IActionResult Get(int id)
    {
      var db = new AppDbContext();
      var p = db.Products.Find(id);

      if (p is null) // idsi veri tabanın olamayan bir istek yapılırsa diye 404 kayıt bulunamadı sinyali döndürüyorum
        return NotFound(); // 404

      return Ok(p); // 200
    }

    [HttpDelete("{id:int}")] // silme işlemi için (DELETE işlemleri GET işlemlerine göre networkde daha az bir maliyet oluşturur.)
    public IActionResult Delete(int id)
    {
      var db = new AppDbContext();
      var entity = db.Products.Find(id);

      if (entity is null) // kayıt yoksa 404 döndür.
        return NotFound();

      //try
      //{
      //  db.Products.Remove(entity);
      //  db.SaveChanges();
      //}
      //catch (Exception ex)
      //{
      //  return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
      //}

    

      return NoContent(); // 204
    }

    // [FromBody] ile formdan json tipinde veri göndereceğimiz anlamına geliyor.

    [HttpPut("{id:int}")] // api/Products/1 PUT api/Products GET
    public IActionResult Update(int id, [FromBody] ProductUpdateDto model)
    {
      var db = new AppDbContext();
      var entity = db.Products.Find(id);

      if (entity is null)
        return NotFound();

      if (ModelState.IsValid)
      {
        entity.Stock = model.Stock;
        entity.Price = (decimal)model.Price;

        db.Products.Update(entity);
        db.SaveChanges();

        return NoContent();
      } 
      else
      {
        // istenilmeyen bir request formatı için kullandık.
        return BadRequest(ModelState); // model state nesnesini 400 hata kodu ile fırlat.
      }

    }


    [HttpPut("changeProductName/{id:int}")] // api/Products/1 PUT api/Products/1 GET, api/Products/changeProductName/1 şeklinde bir like dönüştürüp sorundan kurtulduk.
    public IActionResult ChangeProductName(int id, [FromBody] ChangeProductNameDto model)
    {
      if (ModelState.IsValid)
      {
        var db = new AppDbContext();
        var entity = db.Products.Find(id);

        if(entity is null)
        {
          return NotFound();
        }
        else
        {
          entity.Name = model.Name;
          db.Products.Update(entity);
          db.SaveChanges();
          return NoContent();
        }
      }
      else
      {
        return BadRequest(ModelState);
      }
      
    }


    // Header üzerinden gönderilen değer genelde, kofigurasyon amaçlı ve kontrol amaçlıdır
    // Client Credentials Header üzerinden ClientId ve ClientSecret değerleri Client uygulamanın API uygulama ile bir haberleşme tekniğidir.
    // Bu tarz hassa bilgiler header üzerinden gönderilir. 
    // Veri ekleme, güncelleme, silme gibi işlemler için [FromBody] kullanılır

    [HttpGet("clientCredentials")]
    public IActionResult GetCrendentials([FromHeader] string clientName, [FromHeader] string clientSecret)
    {

      if(clientName == "ReactApp" && clientSecret == "x-secret")
      {
        return Ok("Veriyi göstermeye yetkili");
      }

      return Unauthorized(); // 401 veriyi görüntüleme için yetkin yok.

    }


  }
}
