using Newtonsoft.Json;

namespace SlackIntegration.SlackLibrary
{
    public class SlackMessage
    {
        [JsonProperty(PropertyName = "channel")]
        public string Channel { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
    }
}
