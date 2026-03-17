namespace TandemBackend.Models
{
    public class Topic
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required string TitleRu { get; set; }
        public required string ContentRu { get; set; }
    }

    public class TopicPut
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required string TitleRu { get; set; }
        public required string ContentRu { get; set; }
    }

    public class TopicMono
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
    }

    public class TopicTitle
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
    }
}
