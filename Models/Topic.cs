namespace TandemBackend.Models
{
    public abstract class Topic
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Example { get; set; }
    }

    public class EnTopic : Topic { }

    public class RuTopic : Topic { }

    public class TopicPut
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Example { get; set; }
    }

    public class TopicGetResult
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Example { get; set; }
    }
}
