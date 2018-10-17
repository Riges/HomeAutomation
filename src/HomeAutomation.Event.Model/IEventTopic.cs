using System.Threading.Tasks;

namespace HomeAutomation.Event.Model
{
    public interface IEventTopic
    {
        Task PublishAsync(IDomainEvent domainEvent);
    }
}