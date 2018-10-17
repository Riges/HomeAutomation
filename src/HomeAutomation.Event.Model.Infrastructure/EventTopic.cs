using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace HomeAutomation.Event.Model.Infrastructure
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
                var @event = JsonConvert.SerializeObject(domainEvent);
                var bytes = Encoding.UTF8.GetBytes(@event);

                var props = channel.CreateBasicProperties();
                props.Persistent = true;
                props.ContentType = "application/json";
                props.Headers = new Dictionary<string, object>
                {
                    {"eventId", domainEvent.EventId},
                    {"parentEventId", domainEvent.ParentEventId},
                    {"createdAt", domainEvent.CreatedAt},
                    {"author", domainEvent.Author},
                    {"name", domainEvent.Name},
                    {"id", domainEvent.Id},
                    {"authorId", domainEvent.AuthorId}
                };

                channel.BasicPublish(exchangeName, routingKey, props, bytes);
            });
        }
    }
}