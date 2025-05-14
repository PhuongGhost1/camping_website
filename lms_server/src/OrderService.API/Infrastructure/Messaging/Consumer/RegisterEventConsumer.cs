
using System.Text;
using System.Text.Json;
using OrderService.API.Application.DTOs.Order;
using OrderService.API.Application.Services;
using OrderService.API.Application.Shared.Constant;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OrderService.API.Infrastructure.Messaging.Consumer;
public class RegisterEventConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    public RegisterEventConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory()
        {
            HostName = RabbitMQConst.RABBITMQ_HOST,
            Port = int.Parse(RabbitMQConst.RABBITMQ_PORT),
            UserName = RabbitMQConst.RABBITMQ_USERNAME,
            Password = RabbitMQConst.RABBITMQ_PASSWORD
        };
        
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.ExchangeDeclare("auth_exchange", ExchangeType.Fanout);
        var queueName = channel.QueueDeclare().QueueName;
        channel.QueueBind(queue: queueName, exchange: "auth_exchange", routingKey: "");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var data = JsonSerializer.Deserialize<RegisterEvent>(message);

            using var scope = _serviceProvider.CreateScope();
            var orderService = scope.ServiceProvider.GetRequiredService<IOrderServices>();

            var orderReq = new CreateOrderReq(
                UserId: data.UserId,
                TotalAmount: 0
            );

            var result = orderService.CreateOrder(orderReq).Result;
            if (result is null)
            {
                Console.WriteLine($"[OrderService] Failed to create order for user: {data.UserId}");
            }

            Console.WriteLine($"[OrderService] Received login event for user: {data.UserId}");
        };

        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }
}