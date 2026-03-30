namespace TandemBackend.Models
{
    enum TaskType
    {
        quiz,
        codeCompletion,
        codeEditor,
    }

    enum TaskDifficulty
    {
        easy,
        medium,
        hard,
    }

    class TaskStat
    {
        public required int Id { get; set; }
        public required string UserId { get; set; }
        public required int TaskId { get; set; }
        public required TaskType TaskType { get; set; }
        public required TaskDifficulty TaskDifficulty { get; set; }
    }
}
