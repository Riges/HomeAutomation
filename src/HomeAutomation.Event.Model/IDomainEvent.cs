using System;
using NodaTime;

namespace HomeAutomation.Event.Model
{
    public interface IDomainEvent
    {
        Guid EventId { get; set; }
        Guid? ParentEventId { get; set; }
        string Name { get; set; }
        Guid? Id { get; set; }
        DomainEventAuthor Author { get; set; }
        Guid? AuthorId { get; set; }
        Instant CreatedAt { get; set; }
    }
}