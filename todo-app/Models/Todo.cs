using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace todo_app.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required bool IsCompleted { get; set; }
        public required string UserId { get; set; }
        public IdentityUser User { get; set; }
    }

    public class TodoContext : IdentityDbContext<IdentityUser>
    {
        private readonly IConfiguration _configuration;

        public TodoContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<Todo> Todos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 32)));
        }
    }
}
