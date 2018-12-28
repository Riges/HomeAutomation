using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HomeAutomation.Event.Model;
using Newtonsoft.Json;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using RabbitMQ.Client;

namespace HomeAutomation.Event.Infrastructure
{
    public class EventTopic : IEventTopic, IDisposable
    {
        private readonly IModel channel;
        private readonly string exchangeName;
        private readonly string routingKey;

        public EventTopic(IConnection connection, string exchangeName, string routingKey)
        {
            this.exchangeName = exchangeName;
            this.routingKey = routingKey;
            channel = connection.CreateModel();
        }

        public void Dispose()
        {
            channel?.Dispose();
        }

        public Task PublishAsync(IDomainEvent domainEvent)
        {
            return Task.Run(() =>
            {
                var settings = new JsonSerializerSettings();
                settings.ConfigureForNodaTime(DateTimeZoneProviders.Serialization);
                var @event = JsonConvert.SerializeObject(domainEvent, settings);
                var bytes = Encoding.UTF8.GetBytes(@event);

                var props = channel.CreateBasicProperties();
                props.Persistent = true;
                props.ContentType = "application/json";
                props.Headers = new Dictionary<string, object>
                {
                    {"eventId", domainEvent.EventId.ToString()},
                    {"parentEventId", domainEvent.ParentEventId.ToString()},
                    {"createdAt", domainEvent.CreatedAt.ToString()},
                    {"author", domainEvent.Author.ToString()},
                    {"name", domainEvent.Name},
                    {"id", domainEvent.Id.ToString()},
                    {"authorId", domainEvent.AuthorId.ToString()}
                };

                channel.BasicPublish(exchangeName, routingKey, props, bytes);
            });
        }
    }
}