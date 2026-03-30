using System.ComponentModel;
using System.Text.Json.Serialization;

namespace TandemBackend.Models
{
    [Description("Enumerator contains {en,ru}")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Languages
    {
        en,
        ru,
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TaskType
    {
        quiz,
        codeCompletion,
        codeEditor,
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TaskDifficulty
    {
        easy,
        medium,
        hard,
    }
}
