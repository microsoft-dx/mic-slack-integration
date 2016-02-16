using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SlackIntegration.Hubs;
using SlackIntegration.SlackLibrary;
using System.Configuration;
using System.Web.Http;

namespace SlackIntegration.Controllers
{
    public class SlackController : ApiController
    {
        public ISlackClient SlackClient { get; set; }
        public IConnectionManager ConnectionManager {get;set;}

        [HttpPost]
        public void PostMessageToSlack(SlackMessage message)
        {
            SlackClient.PostMessage(message);
            ConnectionManager.GetHubContext<SlackHub>().Clients.All.addMessage(message.UserName, message.Text);
        }

        [HttpPost]
        public void PostMessageToWeb([FromBody]JToken commandRequest)
        {
            var command = JsonConvert.DeserializeObject<SlackCommand>(commandRequest.ToString());

            if(command.Token == ConfigurationManager.AppSettings["SlackCommandToken"])
            {
                ConnectionManager.GetHubContext<SlackHub>().Clients.All.addMessage(command.UserName, command.Text);
                SlackClient.PostMessage(text: command.Text, userName: command.UserName);
            }

        }
    }
}
