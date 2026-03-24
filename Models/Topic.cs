using System.ComponentModel;

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
        [Description("Title on english")]
        public required string Title { get; set; }

        [Description("Content on english")]
        public required string Content { get; set; }

        [Description("Title on russian")]
        public required string TitleRu { get; set; }

        [Description("Content on russian")]
        public required string ContentRu { get; set; }
    }

    [Description("Short version with one chosen language")]
    public class TopicMono
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
    }

    [Description("Contains only id and title on chosen language")]
    public class TopicTitle
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
    }
}
