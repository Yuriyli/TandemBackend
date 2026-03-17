namespace TandemBackend.Models
{
    public class Article
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required string TitleRu { get; set; }
        public required string ContentRu { get; set; }
    }

    public class ArticlePut
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required string TitleRu { get; set; }
        public required string ContentRu { get; set; }
    }

    public class ArticleGet
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
    }
}
