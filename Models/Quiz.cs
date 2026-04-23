using System.ComponentModel.DataAnnotations;

namespace TandemBackend.Models
{
    public class Quiz : ITask<QuizQuestion>
    {
        public required int Id { get; set; }
        public required string Name { get; set; }

        public required int PracticeTopicId { get; set; }
        public required PracticeTopic PracticeTopic { get; set; }

        public required string Title { get; set; }
        public required string Description { get; set; }
        public required IEnumerable<QuizQuestion> Questions { get; set; }

        public QuizRu? QuizRu { get; set; }
    }

    public class QuizRu
    {
        public required int Id { get; set; }

        public required int QuizId { get; set; }
        public required Quiz Quiz { get; set; }

        public required string Title { get; set; }
        public required string Description { get; set; }
        public required IEnumerable<QuizQuestionRu> Questions { get; set; }
    }

    public class QuizQuestion : IQuestion
    {
        public required int Id { get; set; }

        public required int QuizId { get; set; }
        public required Quiz Quiz { get; set; }

        public required string Question { get; set; }
        public required string CorrectAnswer { get; set; }
        public required IEnumerable<string> Options { get; set; }
    }

    public class QuizQuestionRu
    {
        public required int Id { get; set; }

        public required int QuizRuId { get; set; }
        public required QuizRu QuizRu { get; set; }

        public required string Question { get; set; }
        public required string CorrectAnswer { get; set; }
        public required IEnumerable<string> Options { get; set; }
    }

    public class QuizPost
    {
        public required string Name { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }

        [MinLength(10)]
        [MaxLength(10)]
        public required IEnumerable<QuizQuestionPost> Questions { get; set; }
    }

    public class QuizQuestionPost
    {
        public required string Question { get; set; }
        public required string CorrectAnswer { get; set; }

        [MinLength(3)]
        public required IEnumerable<string> Options { get; set; }
    }

    public class QuizGetResult
    {
        public required int Id { get; set; }
        public required int PracticeTopicId { get; set; }

        public required string Title { get; set; }
        public required string Description { get; set; }
        public required IEnumerable<QuizQuestionPost> Questions { get; set; }
    }

    public class QuizQuestionGetResult
    {
        public required int Id { get; set; }
        public required string Question { get; set; }
        public required string CorrectAnswer { get; set; }
        public required IEnumerable<string> Options { get; set; }
    }

    public class QuizPut
    {
        public required string Name { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }

        [MinLength(10)]
        [MaxLength(10)]
        public required IEnumerable<QuizQuestionPost> Questions { get; set; }
    }

    public class QuizQuestionPut
    {
        public required string Question { get; set; }
        public required string CorrectAnswer { get; set; }

        [MinLength(3)]
        public required IEnumerable<string> Options { get; set; }
    }
}
