using System.ComponentModel.DataAnnotations;

namespace TandemBackend.Models
{
    public class CodeEditor : ITask<CodeEditorQuestion>
    {
        public required int Id { get; set; }
        public required string Name { get; set; }

        public required int PracticeTopicId { get; set; }
        public required PracticeTopic PracticeTopic { get; set; }

        public required string Title { get; set; }
        public required string Description { get; set; }
        public required IEnumerable<CodeEditorQuestion> Questions { get; set; }

        public CodeEditorRu? CodeEditorRu { get; set; }
    }

    public class CodeEditorRu
    {
        public required int Id { get; set; }

        public required int CodeEditorId { get; set; }
        public required CodeEditor CodeEditor { get; set; }

        public required string Title { get; set; }
        public required string Description { get; set; }
        public required IEnumerable<CodeEditorQuestionRu> Questions { get; set; }
    }

    public class CodeEditorQuestion : IQuestion
    {
        public required int Id { get; set; }

        public required int CodeEditorId { get; set; }
        public required CodeEditor CodeEditor { get; set; }

        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Instructions { get; set; }
        public required string StarterCode { get; set; }
        public string? Hint { get; set; }
        public required IEnumerable<string> ExpectedAnswers { get; set; }
    }

    public class CodeEditorQuestionRu
    {
        public required int Id { get; set; }

        public required int CodeEditorRuId { get; set; }
        public required CodeEditorRu CodeEditorRu { get; set; }

        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Instructions { get; set; }
        public required string StarterCode { get; set; }
        public string? Hint { get; set; }
        public required IEnumerable<string> ExpectedAnswers { get; set; }
    }

    public class CodeEditorPost
    {
        public required string Name { get; set; }

        public required string Title { get; set; }
        public required string Description { get; set; }

        [MinLength(1)]
        [MaxLength(1)]
        public required IEnumerable<CodeEditorQuestionPost> Questions { get; set; }
    }

    public class CodeEditorQuestionPost
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Instructions { get; set; }
        public required string StarterCode { get; set; }
        public string? Hint { get; set; }

        [MinLength(1)]
        public required IEnumerable<string> ExpectedAnswers { get; set; }
    }

    public class CodeEditorGetResult
    {
        public required int Id { get; set; }
        public required int PracticeTopicId { get; set; }

        public required string Title { get; set; }
        public required string Description { get; set; }
        public required IEnumerable<CodeEditorQuestionGetResult> Questions { get; set; }
    }

    public class CodeEditorQuestionGetResult
    {
        public required int Id { get; set; }

        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Instructions { get; set; }
        public required string StarterCode { get; set; }
        public string? Hint { get; set; }
        public required IEnumerable<string> ExpectedAnswers { get; set; }
    }

    public class CodeEditorPut
    {
        public required string Name { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }

        [MinLength(1)]
        [MaxLength(1)]
        public required IEnumerable<CodeEditorQuestionPut> Questions { get; set; }
    }

    public class CodeEditorQuestionPut
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Instructions { get; set; }
        public required string StarterCode { get; set; }
        public string? Hint { get; set; }

        [MinLength(1)]
        public required IEnumerable<string> ExpectedAnswers { get; set; }
    }
}
