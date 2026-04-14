namespace TandemBackend.Models
{
    public class Lesson
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Title { get; set; }
        public LessonRu? LessonRu { get; set; }
    }

    public class LessonRu
    {
        public required int Id { get; set; }
        public required int LessonId { get; set; }
        public required Lesson Lesson { get; set; }
        public required string Title { get; set; }
    }

    public class LessonPost
    {
        public required string Name { get; set; }
        public required string Title { get; set; }
    }

    public class LessonGetResult
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Title { get; set; }
    }

    public class LessonPut
    {
        public required string Name { get; set; }
        public required string Title { get; set; }
    }
}
