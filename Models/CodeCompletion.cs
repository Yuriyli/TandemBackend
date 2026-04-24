using System.ComponentModel.DataAnnotations;

namespace TandemBackend.Models
{
    public class CodeCompletion : ITask<CodeCompletionQuestion>
    {
        public required int Id { get; set; }
        public required string Name { get; set; }

        public required int PracticeTopicId { get; set; }
        public required PracticeTopic PracticeTopic { get; set; }

        public required string Title { get; set; }
        public required string Description { get; set; }
        public required IEnumerable<CodeCompletionQuestion> Questions { get; set; }

        public CodeCompletionRu? CodeCompletionRu { get; set; }
    }

    public class CodeCompletionRu
    {
        public required int Id { get; set; }

        public required int CodeCompletionId { get; set; }
        public required CodeCompletion CodeCompletion { get; set; }

        public required string Title { get; set; }
        public required string Description { get; set; }
        public required IEnumerable<CodeCompletionQuestionRu> Questions { get; set; }
    }

    public class CodeCompletionQuestion : IQuestion
    {
        public required int Id { get; set; }

        public required int CodeCompletionId { get; set; }
        public required CodeCompletion CodeCompletion { get; set; }

        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Code { get; set; }
        public required string CorrectAnswer { get; set; }
        public string? Hint { get; set; }
        public required IEnumerable<string> Options { get; set; }
    }

    public class CodeCompletionQuestionRu
    {
        public required int Id { get; set; }

        public required int CodeCompletionRuId { get; set; }
        public required CodeCompletionRu CodeCompletionRu { get; set; }

        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Code { get; set; }
        public required string CorrectAnswer { get; set; }
        public string? Hint { get; set; }
        public required IEnumerable<string> Options { get; set; }
    }

    public class CodeCompletionPost
    {
        public required string Name { get; set; }

        public required string Title { get; set; }
        public required string Description { get; set; }

        [MinLength(10)]
        [MaxLength(10)]
        public required IEnumerable<CodeCompletionQuestionPost> Questions { get; set; }
    }

    public class CodeCompletionQuestionPost
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Code { get; set; }
        public required string CorrectAnswer { get; set; }
        public string? Hint { get; set; }

        [MinLength(3)]
        public required IEnumerable<string> Options { get; set; }
    }

    public class CodeCompletionGetResult
    {
        public required int Id { get; set; }
        public required int PracticeTopicId { get; set; }

        public required string Title { get; set; }
        public required string Description { get; set; }
        public required IEnumerable<CodeCompletionQuestionGetResult> Questions { get; set; }
    }

    public class CodeCompletionQuestionGetResult
    {
        public required int Id { get; set; }

        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Code { get; set; }
        public required string CorrectAnswer { get; set; }
        public string? Hint { get; set; }
        public required IEnumerable<string> Options { get; set; }
    }

    public class CodeCompletionPut
    {
        public required string Name { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }

        [MinLength(10)]
        [MaxLength(10)]
        public required IEnumerable<CodeCompletionQuestionPut> Questions { get; set; }
    }

    public class CodeCompletionQuestionPut
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Code { get; set; }
        public required string CorrectAnswer { get; set; }
        public string? Hint { get; set; }

        [MinLength(3)]
        public required IEnumerable<string> Options { get; set; }
    }
}
