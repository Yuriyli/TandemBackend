namespace TandemBackend.Models
{
    public class TaskStat
    {
        public required int Id { get; set; }
        public required string UserId { get; set; }
        public required int TaskId { get; set; }
        public required bool IsFinished { get; set; }
        public required TaskType TaskType { get; set; }
        public required TaskDifficulty TaskDifficulty { get; set; }
    }

    public class TaskStatPost
    {
        public required int TaskId { get; set; }
        public required bool IsFinished { get; set; }
        public required TaskType TaskType { get; set; }
        public required TaskDifficulty TaskDifficulty { get; set; }
    }

    public class TaskStatGetResult
    {
        public required int TaskId { get; set; }
        public required TaskType TaskType { get; set; }
        public required TaskDifficulty TaskDifficulty { get; set; }
    }
}
