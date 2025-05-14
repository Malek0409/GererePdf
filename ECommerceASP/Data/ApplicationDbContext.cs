using ECommerceASP.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceASP.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, name = "Chaussure" },
                new Category { Id = 2, name = "Pantalon" },
                new Category { Id = 3, name = "Chemise" }
            );
           

            
            modelBuilder.Entity<Product>().HasData(
          
                new Product { Id = 1, Name = "Nike Air", Price = 120.00m, CategoryId = 1 },
                new Product { Id = 2, Name = "Adidas Run", Price = 100.00m, CategoryId = 1 },
                new Product { Id = 3, Name = "Puma Sport", Price = 90.00m, CategoryId = 1 },
                new Product { Id = 4, Name = "Reebok Classic", Price = 110.00m, CategoryId = 1 },
                new Product { Id = 5, Name = "Converse All Star", Price = 80.00m, CategoryId = 1 },

              
                new Product { Id = 6, Name = "Jean Slim", Price = 60.00m, CategoryId = 2 },
                new Product { Id = 7, Name = "Chino Beige", Price = 55.00m, CategoryId = 2 },
                new Product { Id = 8, Name = "Jogging Noir", Price = 40.00m, CategoryId = 2 },
                new Product { Id = 9, Name = "Short Sport", Price = 35.00m, CategoryId = 2 },
                new Product { Id = 10, Name = "Pantalon Cargo", Price = 70.00m, CategoryId = 2 },

                
                new Product { Id = 11, Name = "Chemise Blanche", Price = 45.00m, CategoryId = 3 },
                new Product { Id = 12, Name = "Chemise à Carreaux", Price = 50.00m, CategoryId = 3 },
                new Product { Id = 13, Name = "Chemise Jean", Price = 55.00m, CategoryId = 3 },
                new Product { Id = 14, Name = "Chemise Lin", Price = 60.00m, CategoryId = 3 },
                new Product { Id = 15, Name = "Chemise Noire", Price = 48.00m, CategoryId = 3 }
            );

            modelBuilder.Entity<Product>()
               .Property(p => p.Price)
               .HasPrecision(18, 2);
        }


    }
}
