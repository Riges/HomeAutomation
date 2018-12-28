using System;

namespace HomeAutomation.Referential.Models
{
    public interface IThing
    {
        Guid Id { get; }
        string Label { get; }
    }
}