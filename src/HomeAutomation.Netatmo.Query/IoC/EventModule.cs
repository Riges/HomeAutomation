using System;
using Autofac;
using HomeAutomation.Event.Model;
using HomeAutomation.Event.Infrastructure;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace HomeAutomation.Netatmo.Query.IoC
{
    public class EventModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var configuration = c.Resolve<IConfiguration>().GetSection("RabbitMQ");
                var factory = new ConnectionFactory
                {
                    UserName = configuration.GetSection("Username").Value,
                    Password = configuration.GetSection("Password").Value,
                    VirtualHost = configuration.GetSection("VirtualHost").Value,
                    HostName = configuration.GetSection("Hostname").Value,
                    Port = Convert.ToInt32(configuration.GetSection("Port").Value)
                };

                return factory.CreateConnection();
            }).As<IConnection>();

            builder.Register(c =>
            {
                var configuration = c.Resolve<IConfiguration>().GetSection("RabbitMQ");
                return new EventTopic(c.Resolve<IConnection>(), configuration.GetSection("Exchange").Value, "");
            }).As<IEventTopic>();
        }
    }
}