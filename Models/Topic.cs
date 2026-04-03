namespace TandemBackend.Models
{
    public class Topic
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Example { get; set; }
    }

    public class TopicPut
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Example { get; set; }
    }

    public class TopicGet
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Example { get; set; }
    }
}
