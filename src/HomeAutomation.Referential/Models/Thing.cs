using System;

namespace HomeAutomation.Referential.Models
{
    public abstract class Thing : IThing
    {
        protected Thing()
        {
        }

        protected Thing(Guid id, string label)
        {
            Id = id;
            Label = label;
        }

        public Guid Id { get; set; }
        public string Label { get; set; }
    }
}