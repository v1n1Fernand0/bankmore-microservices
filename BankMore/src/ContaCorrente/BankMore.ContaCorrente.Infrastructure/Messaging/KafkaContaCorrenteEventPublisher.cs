using BankMore.Application.Abstractions;
using BankMore.Application.Events;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace BankMore.Infrastructure.Messaging
{
    public class KafkaContaCorrenteEventPublisher : IContaCorrenteEventPublisher
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic = "conta-corrente-criada";

        public KafkaContaCorrenteEventPublisher(IConfiguration config)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = config.GetSection("Kafka:BootstrapServers").Value
            };

            _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        }

        public async Task PublicarAsync(ContaCorrenteCriadaEvent evento)
        {
            var json = JsonSerializer.Serialize(evento);
            await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = json });
        }
    }
}
