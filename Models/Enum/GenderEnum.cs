using System.Text.Json.Serialization;

namespace APIBlog.Models.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum GenderEnum
    {
        Male, Female
    }
}