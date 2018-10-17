using System;
using HomeAutomation.Event.Model;
using NodaTime;

namespace HomeAutomation.Event.Weather
{
    public class TemperatureMeasureEvent : MeasureEventBase
    {
        public TemperatureMeasureEvent(DomainEventAuthor author, Instant createdAt, Instant measuredAt, Guid? deviceId, Guid? roomId, double temperature)
            : this(author, createdAt, null, null, null, measuredAt, deviceId, roomId, temperature)
        {
        }

        public TemperatureMeasureEvent(DomainEventAuthor author, Instant createdAt, Guid? id, Guid? authorId, Guid? parentEventId,
            Instant measuredAt, Guid? deviceId, Guid? roomId, double temperature)
            : base(nameof(TemperatureMeasureEvent), author, createdAt, id, authorId, parentEventId, measuredAt, deviceId, roomId)
        {
            Temperature = temperature;
        }

        public double Temperature { get; set; }
    }
}