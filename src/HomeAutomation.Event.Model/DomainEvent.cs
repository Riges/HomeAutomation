using System;
using NodaTime;

namespace HomeAutomation.Event.Model
{
    public abstract class DomainEvent : IDomainEvent
    {
        protected DomainEvent()
        {
            EventId = Guid.NewGuid();
        }

        protected DomainEvent(string name, DomainEventAuthor author)
            : this()
        {
            Name = name;
            Author = author;
        }

        protected DomainEvent(string name, DomainEventAuthor author, Instant createdAt, Guid? id, Guid? authorId)
            : this(name, author)
        {
            CreatedAt = createdAt;
            Id = id;
            AuthorId = authorId;
        }

        protected DomainEvent(string name, DomainEventAuthor author, Instant createdAt, Guid? id, Guid? authorId, Guid? parentEventId) :
            this(name, author, createdAt, id, authorId)
        {
            ParentEventId = parentEventId;
        }

        public Guid EventId { get; set; }
        public Guid? ParentEventId { get; set; }
        public string Name { get; set; }
        public Guid? Id { get; set; }
        public DomainEventAuthor Author { get; set; }
        public Guid? AuthorId { get; set; }
        public Instant CreatedAt { get; set; }
    }
}