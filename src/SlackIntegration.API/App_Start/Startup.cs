using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin;
using Owin;
using SlackIntegration.Utils;
using System.Web.Http;

[assembly: OwinStartup(typeof(SlackIntegration.Startup))]

namespace SlackIntegration
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            WebApiConfiguration(config);

            var container = new AutofacContainer().Container;

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
            app.UseWebApi(config);

            var resolver = new AutofacDependencyResolver(container);
            app.MapSignalR(new HubConfiguration
            {
                Resolver = resolver,
                EnableDetailedErrors = true
            });

            ConfigureSignalR(container, resolver);
        }

        private void ConfigureSignalR(IContainer container, IDependencyResolver resolver)
        {
            var newBuilder = new ContainerBuilder();
            newBuilder.RegisterInstance(resolver.Resolve<IConnectionManager>());

            newBuilder.Update(container);
        }

        private void WebApiConfiguration(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
