using Microsoft.EntityFrameworkCore;

namespace MinimalShoppingListAPI
{
    public class ApiDbContext : DbContext
    {
        public DbSet<Grocery> Groceries => Set<Grocery>();
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
                
        }


    }
}
