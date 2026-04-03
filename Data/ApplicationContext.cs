using Microsoft.EntityFrameworkCore;
using TandemBackend.Models;

namespace TandemBackend.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<ExampleModel> ExampleModels => Set<ExampleModel>();
        public DbSet<Topic> EnTopics => Set<Topic>();
        public DbSet<Topic> RuTopics => Set<Topic>();
        public DbSet<AppUser> Users => Set<AppUser>();
        public DbSet<TaskStat> TaskStats => Set<TaskStat>();

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options) { }
    }
}
