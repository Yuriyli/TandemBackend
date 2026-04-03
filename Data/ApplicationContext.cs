using Microsoft.EntityFrameworkCore;
using TandemBackend.Models;

namespace TandemBackend.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<EnTopic> EnTopics => Set<EnTopic>();
        public DbSet<RuTopic> RuTopics => Set<RuTopic>();
        public DbSet<AppUser> Users => Set<AppUser>();
        public DbSet<TaskStat> TaskStats => Set<TaskStat>();

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options) { }
    }
}
