using BankMore.Application.EventHandlers;
using BankMore.Application.Events;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BankMore.Infrastructure.Messaging;

public class ContaCorrenteCriadaConsumer : BackgroundService
{
    private readonly ILogger<ContaCorrenteCriadaConsumer> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _config;

    public ContaCorrenteCriadaConsumer(
        ILogger<ContaCorrenteCriadaConsumer> logger,
        IServiceProvider serviceProvider,
        IConfiguration config)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _config = config;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _config["Kafka:BootstrapServers"],
            GroupId = "usuario-api-consumer",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe("conta-corrente-criada");

        _logger.LogInformation("Consumer iniciado e escutando o tópico 'conta-corrente-criada'");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = consumer.Consume(stoppingToken);
                var evento = JsonSerializer.Deserialize<ContaCorrenteCriadaEvent>(result.Message.Value);

                if (evento is null)
                {
                    _logger.LogWarning("Evento nulo recebido");
                    continue;
                }

                using var scope = _serviceProvider.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<IContaCorrenteCriadaHandler>();
                await handler.ProcessarAsync(evento);

                _logger.LogInformation($"Evento processado: Conta {evento.NumeroConta} para usuário {evento.IdUsuario}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar evento ContaCorrenteCriada");
            }
        }
    }
}
