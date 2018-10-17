using System;
using System.Threading.Tasks;
using HomeAutomation.Referential.Models;

namespace HomeAutomation.Referential.Infrastructure
{
    public interface IReferential
    {
        Task<T> GetThing<T>(Guid id) where T : Thing;

        Task<Device> GetDeviceByProviderId(string providerId);
    }
}