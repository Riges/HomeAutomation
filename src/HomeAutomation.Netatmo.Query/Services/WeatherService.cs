using System.Threading.Tasks;
using HomeAutomation.Event.Model;
using HomeAutomation.Event.Weather;
using HomeAutomation.Referential.Fsharp;
using HomeAutomation.Referential.Infrastructure;
using Netatmo.Models.Client.Weather.StationsData.DashboardData;
using NodaTime;
using Device = Netatmo.Models.Client.Weather.StationsData.Device;

namespace HomeAutomation.Netatmo.Query.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IClock clock;
        private readonly IEventTopic eventTopic;
        private readonly IReferential referential;
        private readonly ReferentialRecord referentialFsharp;

        public WeatherService(IClock clock, IEventTopic eventTopic, IReferential referential, ReferentialRecord referentialFsharp)
        {
            this.clock = clock;
            this.eventTopic = eventTopic;
            this.referential = referential;
            this.referentialFsharp = referentialFsharp;
        }

        public async Task SendWeatherInformations(Device station)
        {
            await SendTemperatureMeasureEvent(station.Id, station.DashboardData.TimeUtc, station.DashboardData.Temperature);

            foreach (var module in station.Modules)
            {
                if (module.Type == "NAMain")
                {
                    var dashboardData = module.GetDashboardData<BaseStationDashBoardData>();
                    await SendTemperatureMeasureEvent(module.Id, dashboardData.TimeUtc, dashboardData.Temperature);
                }

                if (module.Type == "NAModule1")
                {
                    var dashboardData = module.GetDashboardData<OutdoorDashBoardData>();
                    await SendTemperatureMeasureEvent(module.Id, dashboardData.TimeUtc, dashboardData.Temperature);
                }

                if (module.Type == "NAModule4")
                {
                    var dashboardData = module.GetDashboardData<IndoorDashBoardData>();
                    await SendTemperatureMeasureEvent(module.Id, dashboardData.TimeUtc, dashboardData.Temperature);
                }
            }
        }

        private async Task SendTemperatureMeasureEvent(string id, Instant measuredAtUTC, double temperature)
        {
            var device = await referential.GetDeviceByProviderId(id);

            var testDevice = Referential.Fsharp.ConfigurationReferential.getDeviceByProviderId(referentialFsharp, id);

            await eventTopic.PublishAsync(new TemperatureMeasureEvent(
                DomainEventAuthor.Netatmo,
                clock.GetCurrentInstant(),
                measuredAtUTC,
                device?.Id,
                device?.RoomId,
                temperature));
        }
    }
}