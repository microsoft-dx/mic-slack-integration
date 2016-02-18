using Newtonsoft.Json;
using System;

namespace SlackIntegration.SlackLibrary
{
    public class SlackMessage
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public DateTime TimeStamp { get; set; }

        [JsonProperty(PropertyName = "channel")]
        public string Channel { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
    }
}
