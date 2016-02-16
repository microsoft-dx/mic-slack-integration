using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Net;

namespace SlackIntegration.SlackLibrary
{
    public class SlackClient : ISlackClient
    {
        public Uri Uri { get; private set; }

        public SlackClient(string uriString)
        {
            Uri = new Uri(uriString);
        }

        public void PostMessage(string text, string userName = null, string channel = null)
        {
            SlackMessage message = new SlackMessage()
            {
                Channel = channel,
                UserName = userName,
                Text = text
            };

            PostMessage(message);
        }

        public void PostMessage(SlackMessage message)
        {
            string jsonMessage = JsonConvert.SerializeObject(message);

            using(WebClient webClient = new WebClient())
            {
                NameValueCollection data = new NameValueCollection();
                data.Add("payload", jsonMessage);

                var response = webClient.UploadValues(Uri, "POST", data);

                //TODO - do something with the responseText
                //string responseText = webClient.Encoding.GetString(response);
            }
        }

    }
}
