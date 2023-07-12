using Confluent.Kafka;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using static Confluent.Kafka.ConfigPropertyNames;

namespace Hubtel_payment_API.KafkaConfig
{
    public class KafkaConfigurations : IKafkaConfiguration
    {
        public async Task StartConfigurationAsync(string metrics)
        {
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            using var producera = new ProducerBuilder<Null, string>(config).Build();

            var response = await producera.ProduceAsync("ProducerMessage", new Message<Null, string> { Value = metrics });
            Console.WriteLine($"deliverd: {response.Value} to {response.TopicPartitionOffset}");

        }
    }


   

}
