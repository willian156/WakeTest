using Microsoft.EntityFrameworkCore;
using WakeTest.Domain.Entities;

namespace WakeTest.Infrastructure.Repositories
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
    }
}
