using Newtonsoft.Json;

namespace UiTest
{
    /// <summary>
    /// Command model
    /// </summary>
    public  class Command
    {
        [JsonProperty("type")]
        public CommandType CommandType { get; set; }
        [JsonProperty("location")]
        public Point Location { get; set; }
        [JsonProperty("value")]
        public object Value { get; set; }
    }
}
