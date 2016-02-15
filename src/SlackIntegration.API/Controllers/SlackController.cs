using SlackIntegration.SlackLibrary;
using System.Configuration;
using System.Web.Http;

namespace SlackIntegration.Controllers
{
    public class SlackController : ApiController
    {
        private SlackClient SlackClient = new SlackClient(ConfigurationManager.AppSettings["SlackWebHookUri"]);

        [HttpPost]
        public void PostMessage(Message message)
        {
            SlackClient.PostMessage(message);
        }
    }
}
