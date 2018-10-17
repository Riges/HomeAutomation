using System;
using System.Threading.Tasks;
using Netatmo;
using Newtonsoft.Json;

namespace HomeAutomation.Netatmo.Query.Services
{
    public class NetatmoService : INetatmoService
    {
        private readonly IClient client;

        public NetatmoService(IClient client, string username, string password)
        {
            this.client = client;
            this.client.GenerateToken(
                username,
                password,
                new[]
                {
                    Scope.CameraAccess, Scope.CameraRead, Scope.CameraWrite, Scope.HomecoachRead, Scope.PresenceAccess, Scope.PresenceRead,
                    Scope.StationRead, Scope.StationWrite, Scope.ThermostatRead
                }).GetAwaiter().GetResult();
        }

        public async Task RetrieveData()
        {
            Console.WriteLine("Stations data :");
            var stationsData = await client.Weather.GetStationsData();
            Console.WriteLine(JsonConvert.SerializeObject(stationsData, Formatting.Indented));
        }
    }

    public interface INetatmoService
    {
        Task RetrieveData();
    }
}