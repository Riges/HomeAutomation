using System;
using System.Linq;
using Autofac;
using HomeAutomation.Event.Model;
using HomeAutomation.Netatmo.Query.Services;
using HomeAutomation.Referential.Configuration;
using HomeAutomation.Referential.Fsharp;
using HomeAutomation.Referential.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.FSharp.Collections;
using Netatmo;
using NodaTime;
using ConfigurationReferential = HomeAutomation.Referential.Infrastructure.ConfigurationReferential;

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
                        c.Resolve<IClock>(),
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

            builder.Register(c => Referential.Fsharp.ConfigurationReferential.createReferential(
                ListModule.OfSeq(c.Resolve<IConfiguration>().GetValue("Referential:Devices", new Device[0])),
                ListModule.OfSeq(c.Resolve<IConfiguration>().GetValue("Referential:Rooms", new Room[0])),
                MapModule.OfArray(c.Resolve<IConfiguration>().GetValue("NewReferentialMapping", new ThingProviderMapping[0])
                    .Select(m => new Tuple<string, ThingId>(m.providerId, m.thingId)).ToArray()))).As<ReferentialRecord>().SingleInstance();

            builder.Register(c => new WeatherService(
                    c.Resolve<IClock>(),
                    c.Resolve<IEventTopic>(),
                    c.Resolve<IReferential>(),
                    c.Resolve<ReferentialRecord>()))
                .As<IWeatherService>();
        }
    }
}