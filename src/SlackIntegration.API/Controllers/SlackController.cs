using Microsoft.AspNet.SignalR.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SlackIntegration.DAL;
using SlackIntegration.Hubs;
using SlackIntegration.SlackLibrary;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;

namespace SlackIntegration.Controllers
{
    public class SlackController : ApiController
    {
        public ISlackClient SlackClient { get; set; }
        public IConnectionManager ConnectionManager { get; set; }
        public ISlackMessageStore SlackMessageStore { get; set; }

        [HttpPost]
        public void PostMessageToSlack(SlackMessage message)
        {
            SlackClient.PostMessage(message);
            ConnectionManager.GetHubContext<SlackHub>().Clients.All.addMessage(message.UserName, message.Text);
            SlackMessageStore.SaveMessage(message);
        }

        [HttpPost]
        public void PostMessageToWeb([FromBody]JToken commandRequest)
        {
            var command = JsonConvert.DeserializeObject<SlackCommand>(commandRequest.ToString());

            if(command.Token == ConfigurationManager.AppSettings["SlackCommandToken"])
            {
                ConnectionManager.GetHubContext<SlackHub>().Clients.All.addMessage(command.UserName, command.Text);
                SlackClient.PostMessage(text: command.Text, userName: command.UserName);
                SlackMessageStore.SaveMessage(text: command.Text, userName: command.UserName);
            }

        }
        
        [HttpGet]
        public List<SlackMessage> GetMessages(int messagesCount)
        {
            return SlackMessageStore.GetLastMessages(messagesCount);
        }
    }
}
