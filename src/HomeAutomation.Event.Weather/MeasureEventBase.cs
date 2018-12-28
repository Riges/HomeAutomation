using System;
using HomeAutomation.Event.Model;
using NodaTime;

namespace HomeAutomation.Event.Weather
{
    public abstract class MeasureEventBase : DomainEvent
    {
        protected MeasureEventBase(string name, DomainEventAuthor author, Instant createdAt, Guid? id, Guid? authorId, Guid? parentEventId,
            Instant measuredAt, Guid? deviceId, Guid? roomId)
            : base(name, author, createdAt, id, authorId, parentEventId)
        {
            MeasuredAt = measuredAt;
            DeviceId = deviceId;
            RoomId = roomId;
        }

        public Instant MeasuredAt { get; set; }
        public Guid? DeviceId { get; set; }
        public Guid? RoomId { get; set; }
    }
}