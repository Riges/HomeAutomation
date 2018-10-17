using System.Threading.Tasks;
using HomeAutomation.Event.Model;
using HomeAutomation.Event.Weather;
using HomeAutomation.Referential.Infrastructure;
using Netatmo.Models.Client.Weather.StationsData;
using Netatmo.Models.Client.Weather.StationsData.DashboardData;
using NodaTime;

namespace HomeAutomation.Netatmo.Query.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IClock clock;
        private readonly IEventTopic eventTopic;
        private readonly IReferential referential;

        public WeatherService(IClock clock, IEventTopic eventTopic, IReferential referential)
        {
            this.clock = clock;
            this.eventTopic = eventTopic;
            this.referential = referential;
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
                    var dashboardData = module.GetDashboardData<BaseStationDashBoardData>();
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