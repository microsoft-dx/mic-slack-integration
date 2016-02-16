using Microsoft.AspNet.SignalR;
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
        private SlackClient SlackClient = new SlackClient(ConfigurationManager.AppSettings["SlackWebHookUri"]);

        [HttpPost]
        public void PostMessageToSlack(SlackMessage message)
        {
            SlackClient.PostMessage(message);
            GlobalHost.ConnectionManager.GetHubContext<SlackHub>().Clients.All.addMessage(message.UserName, message.Text);
        }

        [HttpPost]
        public void PostMessageToWeb([FromBody]JToken commandRequest)
        {
            var command = JsonConvert.DeserializeObject<SlackCommand>(commandRequest.ToString());

            GlobalHost.ConnectionManager.GetHubContext<SlackHub>().Clients.All.addMessage(command.UserName, command.Text);
            SlackClient.PostMessage(text: command.Text, userName: command.UserName);
        }
    }
}
