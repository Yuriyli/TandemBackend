namespace TandemBackend.Models
{
    public class TaskStat
    {
        public required int Id { get; set; }
        public required string UserId { get; set; }
        public required string LessonName { get; set; }
        public required TaskType TaskType { get; set; }
        public required TaskDifficulty Difficulty { get; set; }
        public required int EarnedPoints { get; set; }
        public required int CorrectAnswers { get; set; }
        public required int WrongAnswers { get; set; }
    }

    public class TaskStatPost
    {
        public required string LessonName { get; set; }
        public required TaskType TaskType { get; set; }
        public required TaskDifficulty Difficulty { get; set; }
        public required int EarnedPoints { get; set; }
        public required int CorrectAnswers { get; set; }
        public required int WrongAnswers { get; set; }
    }

    public class TaskStatGetResult
    {
        public required string LessonName { get; set; }
        public required TaskType TaskType { get; set; }
        public required TaskDifficulty Difficulty { get; set; }
        public required int EarnedPoints { get; set; }
        public required int CorrectAnswers { get; set; }
        public required int WrongAnswers { get; set; }
    }
}
