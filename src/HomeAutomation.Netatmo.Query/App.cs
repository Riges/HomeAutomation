using System.Linq;
using System.Threading.Tasks;
using HomeAutomation.Netatmo.Query.Services;
using Netatmo;
using Netatmo.Models.Client.Weather.StationsData;
using NodaTime;

namespace HomeAutomation.Netatmo.Query
{
    public class App
    {
        private readonly IClient client;
        private readonly IClock clock;
        private readonly IWeatherService weatherService;
        private readonly string stationId;

        public App(IClock clock, IClient client, IWeatherService weatherService, string stationId)
        {
            this.clock = clock;
            this.client = client;
            this.stationId = stationId;
            this.weatherService = weatherService;
        }

        public async Task Run(string username, string password)
        {
            await client.GenerateToken(
                username,
                password,
                new[]
                {
                    Scope.CameraAccess, Scope.CameraRead, Scope.CameraWrite, Scope.HomecoachRead, Scope.PresenceAccess, Scope.PresenceRead,
                    Scope.StationRead, Scope.StationWrite, Scope.ThermostatRead
                });

            await HandleWeatherStation();
        }

        public async Task HandleWeatherStation()
        {
            Device station;
            var stationsData = await client.Weather.GetStationsData();

            if (!(stationsData?.Body != null && stationsData.Body.Devices.Any())) return;

            if (!string.IsNullOrWhiteSpace(stationId))
                station = stationsData.Body.Devices.FirstOrDefault(device => device.Id == stationId);
            else
                station = stationsData.Body.Devices.FirstOrDefault();

            if (station == null) return;

            await weatherService.SendWeatherInformations(station);
        }
    }
}