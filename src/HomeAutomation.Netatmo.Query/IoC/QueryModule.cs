using Autofac;
using HomeAutomation.Netatmo.Query.Services;
using HomeAutomation.Referential.Configuration;
using HomeAutomation.Referential.Infrastructure;
using Microsoft.Extensions.Configuration;
using Netatmo;
using NodaTime;

namespace HomeAutomation.Netatmo.Query.IoC
{
    public class QueryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<EventModule>();
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

            builder
                .Register(c => new ConfigurationReferential(
                    c.Resolve<IConfiguration>().GetSection("Referential").Get<Referential.Configuration.Referential>(),
                    c.Resolve<IConfiguration>().GetSection("ReferentialMapping").Get<ReferentialMapping>()))
                .As<IReferential>();

            builder.RegisterType<WeatherService>().As<IWeatherService>();
        }
    }
}