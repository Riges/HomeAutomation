using System;
using System.Linq;
using System.Threading.Tasks;
using HomeAutomation.Referential.Configuration;
using HomeAutomation.Referential.Models;

namespace HomeAutomation.Referential.Infrastructure
{
    public class ConfigurationReferential : IReferential
    {
        private readonly ReferentialMapping mapping;
        private readonly Configuration.Referential referential;

        public ConfigurationReferential(Configuration.Referential referential, ReferentialMapping mapping)
        {
            this.referential = referential;
            this.mapping = mapping;
        }

        public Task<T> GetThing<T>(Guid id) where T : Thing
        {
            if (typeof(T) == typeof(Room))
                return Task.Run(() => (T) Convert.ChangeType(referential.Rooms.FirstOrDefault(room => room.Id == id), typeof(T)));
            if (typeof(T) == typeof(Device))
                return Task.Run(() => (T) Convert.ChangeType(referential.Devices.FirstOrDefault(room => room.Id == id), typeof(T)));

            return null;
        }

        public Task<Device> GetDeviceByProviderId(string providerId)
        {
            return Task.Run(() =>
            {
                var deviceId = mapping.Devices.FirstOrDefault(d => d.ProviderId == providerId)?.ReferentialId;

                return deviceId.HasValue ? referential.Devices.FirstOrDefault(d => d.Id == deviceId.Value) : null;
            });
        }
    }
}