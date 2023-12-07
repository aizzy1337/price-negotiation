using Microsoft.EntityFrameworkCore;
using priceNegotiationAPI.Models;

namespace priceNegotiationAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Negotiation> Negotiations { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product()
                {
                    Id = 1,
                    Name = "Fight Club",
                    Description = "An insomniac office worker and a devil-may-care soap maker form an underground fight club that evolves into much more.",
                    Price = 25.99,
                    CreatedDate = DateTime.Now
                },
                new Product()
                {
                    Id = 2,
                    Name = "Requiem for a Dream",
                    Description = "The drug-induced utopias of four Coney Island people are shattered when their addictions run deep.",
                    Price = 17.99,
                    CreatedDate = DateTime.Now
                }
                );
        }
    }
}
