using Microsoft.EntityFrameworkCore;
using WebApiRaquets.Entities;

namespace WebApiRaquets
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Raquet> Raquets { get; set; }//crear tabla mediante DbSet

        public DbSet<Brand> Brands { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options) 
        {
            
        }
    }
}
