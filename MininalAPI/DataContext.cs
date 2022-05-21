global using Microsoft.EntityFrameworkCore;

namespace MininalAPI
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    }
}
