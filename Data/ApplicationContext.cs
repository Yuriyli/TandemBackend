using Microsoft.EntityFrameworkCore;
using TandemBackend.Models;

namespace TandemBackend.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<ExampleModel> ExampleModels => Set<ExampleModel>();
        public DbSet<Topic> Topics => Set<Topic>();
        public DbSet<AppUser> Users => Set<AppUser>();

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options) { }
    }
}
