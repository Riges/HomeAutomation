using System;
using HomeAutomation.Referential.Enums;

namespace HomeAutomation.Referential.Models
{
    public class Room : Thing
    {
        public Room()
        {
        }
        
        public Room(Guid id, string label, RoomTypeEnum type) : base(id, label)
        {
            Type = type;
        }

        public RoomTypeEnum Type { get; set; }
    }
}