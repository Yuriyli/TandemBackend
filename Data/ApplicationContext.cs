using Microsoft.EntityFrameworkCore;
using TandemBackend.Models;

namespace TandemBackend.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<AppUser> Users => Set<AppUser>();

        public DbSet<TaskStat> TaskStats => Set<TaskStat>();

        public DbSet<EnTopic> EnTopics => Set<EnTopic>();
        public DbSet<RuTopic> RuTopics => Set<RuTopic>();

        public DbSet<Lesson> Lessons => Set<Lesson>();
        public DbSet<LessonRu> LessonsRu => Set<LessonRu>();

        public DbSet<PracticeTopic> PracticeTopic => Set<PracticeTopic>();
        public DbSet<PracticeTopicRu> PracticeTopicRu => Set<PracticeTopicRu>();

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options) { }
    }
}
