namespace TandemBackend.Models
{
    public enum TaskType
    {
        quiz,
        codeCompletion,
        codeEditor,
    }

    public enum TaskDifficulty
    {
        easy,
        medium,
        hard,
    }

    public class TaskStat
    {
        public required int Id { get; set; }
        public required string UserId { get; set; }
        public required int TaskId { get; set; }
        public required TaskType TaskType { get; set; }
        public required TaskDifficulty TaskDifficulty { get; set; }
    }
}
