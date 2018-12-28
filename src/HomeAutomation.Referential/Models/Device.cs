using System;
using HomeAutomation.Referential.Enums;

namespace HomeAutomation.Referential.Models
{
    public class Device : Thing
    {
        public Device()
        {
        }
        
        public Device(Guid id, string label, DeviceTypeEnum type, Guid roomId) : base(id, label)
        {
            Type = type;
            RoomId = roomId;
        }

        public DeviceTypeEnum Type { get; set; }
        public Guid RoomId { get; set; }
    }
}