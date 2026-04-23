namespace TandemBackend.Models
{
    public class PracticeTopic
    {
        public required int Id { get; set; }

        public required int LessonId { get; set; }
        public required Lesson Lesson { get; set; }

        public required string Name { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }

        public PracticeTopicRu? PracticeTopicRu { get; set; }
        // public Quiz? Quiz { get; set; }
        // public CodeCompletion? CodeCompletion{ get; set; }
        // public CodeEditor? CodeEditor{ get; set; }
    }

    public class PracticeTopicRu
    {
        public required int Id { get; set; }

        public required int PracticeTopicId { get; set; }
        public required PracticeTopic PracticeTopic { get; set; }

        public required string Title { get; set; }
        public required string Description { get; set; }
    }

    public class PracticeTopicPost
    {
        public required string Name { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
    }

    public class PracticeTopicGetResult
    {
        public required int Id { get; set; }

        public required int LessonId { get; set; }

        public required string Name { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
    }

    public class PracticeTopicPut
    {
        public required string Name { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
    }
}
