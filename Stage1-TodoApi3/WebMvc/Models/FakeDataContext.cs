using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace WebMvc.Models
{
    public class FakeContext : DbContext
    {
        public FakeContext(DbContextOptions<FakeContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }

        public DbSet<TodoApi.Models.TodoItem> TodoItem { get; set; }
    }
}

