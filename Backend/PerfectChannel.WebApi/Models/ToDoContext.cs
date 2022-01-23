using Microsoft.EntityFrameworkCore;

namespace PerfectChannel.WebApi.Models
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options)
            : base(options)
        {
        }

        public DbSet<ToDoItemModel> ToDoItems { get; set; }
    }
}
