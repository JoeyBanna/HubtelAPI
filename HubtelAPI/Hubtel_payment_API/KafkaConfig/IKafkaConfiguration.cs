using System.Threading.Tasks;

namespace Hubtel_payment_API.KafkaConfig
{
    public interface IKafkaConfiguration
    {
        Task StartConfigurationAsync(string details);
    }
}
