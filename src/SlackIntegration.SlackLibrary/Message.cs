using Newtonsoft.Json;

namespace SlackIntegration.SlackLibrary
{
    public class Message
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
