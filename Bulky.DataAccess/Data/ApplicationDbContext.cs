
using Book.Models;
using Microsoft.EntityFrameworkCore;

namespace Book.DataAccess.Data
{
    public class ApplicationDbContext:DbContext
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Product { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Category>().HasData(
				new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
				new Category { Id = 2, Name = "Sci-Fi", DisplayOrder = 2 },
				new Category { Id = 3, Name = "Self-Help", DisplayOrder = 3 }
				);
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "Edge of Tomorrow",
                    ISBN = "978-0-123456-47-2",
                    Author = "Hiroshi Sakurazaka",
                    ListPrice = 29.99,
                    Price = 27.99,
                    Price50 = 25.99,
                    Price100 = 23.99
                },
new Product
{
    Id = 2,
    Title = "Interstellar: The Science",
    ISBN = "978-0-987654-32-1",
    Author = "Kip Thorne",
    ListPrice = 35.50,
    Price = 32.50,
    Price50 = 30.00,
    Price100 = 28.00
},
new Product
{
    Id = 3,
    Title = "Atomic Habits",
    ISBN = "978-0-123456-01-3",
    Author = "James Clear",
    ListPrice = 25.00,
    Price = 23.00,
    Price50 = 21.00,
    Price100 = 19.00
},
new Product
{
    Id = 4,
    Title = "The 5 AM Club",
    ISBN = "978-0-123456-02-4",
    Author = "Robin Sharma",
    ListPrice = 22.99,
    Price = 20.99,
    Price50 = 18.99,
    Price100 = 16.99
},
new Product
{
    Id = 5,
    Title = "Ready Player One",
    ISBN = "978-0-123456-03-5",
    Author = "Ernest Cline",
    ListPrice = 28.00,
    Price = 26.00,
    Price50 = 24.00,
    Price100 = 22.00
},
new Product
{
    Id = 6,
    Title = "The Martian",
    ISBN = "978-0-123456-04-6",
    Author = "Andy Weir",
    ListPrice = 30.00,
    Price = 28.00,
    Price50 = 26.00,
    Price100 = 24.00
}
);

        }

    }
}
