using Microsoft.EntityFrameworkCore;
using WebAPIIntroduction.Data.Entities;

namespace WebAPIIntroduction.Data
{
  public class AppDbContext:DbContext
  {
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer("Server=(localDB)\\MyLocalDb;Database=ApiDB;Trusted_Connection=True;");
    }
  }
}
