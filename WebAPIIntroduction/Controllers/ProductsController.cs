using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIIntroduction.Data;
using WebAPIIntroduction.Data.Entities;
using WebAPIIntroduction.Dtos;

namespace WebAPIIntroduction.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProductsController : ControllerBase
  {





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
      return Ok();
    }

  }
}
