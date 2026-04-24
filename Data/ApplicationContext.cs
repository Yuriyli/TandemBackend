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

        public DbSet<Quiz> Quiz => Set<Quiz>();
        public DbSet<QuizRu> QuizRu => Set<QuizRu>();
        public DbSet<QuizQuestion> QuizQuestion => Set<QuizQuestion>();
        public DbSet<QuizQuestionRu> QuizQuestionRu => Set<QuizQuestionRu>();

        public DbSet<CodeCompletion> CodeCompletion => Set<CodeCompletion>();
        public DbSet<CodeCompletionRu> CodeCompletionRu => Set<CodeCompletionRu>();
        public DbSet<CodeCompletionQuestion> CodeCompletionQuestion =>
            Set<CodeCompletionQuestion>();
        public DbSet<CodeCompletionQuestionRu> CodeCompletionQuestionRu =>
            Set<CodeCompletionQuestionRu>();

        public DbSet<CodeEditor> CodeEditor => Set<CodeEditor>();
        public DbSet<CodeEditorRu> CodeEditorRu => Set<CodeEditorRu>();
        public DbSet<CodeEditorQuestion> CodeEditorQuestion => Set<CodeEditorQuestion>();
        public DbSet<CodeEditorQuestionRu> CodeEditorQuestionRu => Set<CodeEditorQuestionRu>();

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options) { }
    }
}
