using System.Threading.Tasks;
using Netatmo.Models.Client.Weather.StationsData;

namespace HomeAutomation.Netatmo.Query.Services
{
    public interface IWeatherService
    {
        Task SendWeatherInformations(Device station);
    }
}