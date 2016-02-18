using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using SlackIntegration.DAL;
using SlackIntegration.SlackLibrary;
using System.Configuration;
using System.Reflection;

namespace SlackIntegration.Utils
{
    public class AutofacContainer
    {
        public IContainer Container { get; set; }

        public AutofacContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly())
                .PropertiesAutowired()
                .InstancePerRequest();

            builder.RegisterType<SlackClient>()
                .As<ISlackClient>()
                .PropertiesAutowired()
                .WithParameter("uriString", ConfigurationManager.AppSettings["SlackWebHookUri"]);

            builder.RegisterType<SlackDbContext>()
                .WithParameter("connectionString", ConfigurationManager.ConnectionStrings["SlackDbConnectionString"].ConnectionString)
                .InstancePerRequest();

           builder.RegisterType<SlackMessageStore>()
                .As<ISlackMessageStore>()
                .PropertiesAutowired()
                .InstancePerRequest();

            builder.RegisterHubs(Assembly.GetExecutingAssembly())
                .PropertiesAutowired();

            Container = builder.Build();
        }
    }
}