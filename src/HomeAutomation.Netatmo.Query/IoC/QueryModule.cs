using Autofac;
using HomeAutomation.Netatmo.Query.Services;
using Microsoft.Extensions.Configuration;
using Netatmo;
using NodaTime;

namespace HomeAutomation.Netatmo.Query.IoC
{
    public class QueryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .Register(c =>
                {
                    var configuration = c.Resolve<IConfiguration>().GetSection("Netatmo");
                    return new Client(
                        SystemClock.Instance,
                        configuration.GetSection("ApiUrl").Value,
                        configuration.GetSection("ClientId").Value,
                        configuration.GetSection("ClientSecret").Value);
                })
                .As<IClient>();

            builder.Register(c =>
            {
                var configuration = c.Resolve<IConfiguration>().GetSection("Netatmo");
                return new NetatmoService(
                    c.Resolve<IClient>(),
                    configuration.GetSection("Username").Value,
                    configuration.GetSection("Password").Value);
            }).As<INetatmoService>();
        }
    }
}