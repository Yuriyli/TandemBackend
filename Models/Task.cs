namespace TandemBackend.Models
{
    public interface ITask<TQuestion>
        where TQuestion : IQuestion
    {
        public int Id { get; set; }

        public int PracticeTopicId { get; set; }
        public PracticeTopic PracticeTopic { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<TQuestion> Questions { get; set; }
    }

    public interface IQuestion
    {
        public int Id { get; set; }
    }
}
