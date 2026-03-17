using Microsoft.EntityFrameworkCore;
using TandemBackend.Models;

namespace TandemBackend.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<ExampleModel> ExampleModels => Set<ExampleModel>();
        public DbSet<Article> Articles => Set<Article>();

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
