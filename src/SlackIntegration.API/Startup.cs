using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SlackIntegration.Startup))]

namespace SlackIntegration
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
