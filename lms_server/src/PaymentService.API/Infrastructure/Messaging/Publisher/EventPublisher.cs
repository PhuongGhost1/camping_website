using System.Text;
using System.Text.Json;
using PaymentService.API.Application.Shared.Constant;
using RabbitMQ.Client;

namespace PaymentService.API.Infrastructure.Messaging.Publisher;

public interface IEventPublisher
{
    void PublishPaymentProcessed(Guid? orderId, decimal? total);
}
public class EventPublisher : IEventPublisher
{
    private readonly IConnectionFactory _connectionFactory;
    public EventPublisher()
    {
        _connectionFactory = new ConnectionFactory()
        {
            HostName = RabbitMQConst.RABBITMQ_HOST,
            Port = int.Parse(RabbitMQConst.RABBITMQ_PORT),
            UserName = RabbitMQConst.RABBITMQ_USERNAME,
            Password = RabbitMQConst.RABBITMQ_PASSWORD
        };
    }
    public void PublishPaymentProcessed(Guid? orderId, decimal? total)
    {
        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(exchange: "payment_exchange", type: ExchangeType.Fanout);

        var message = JsonSerializer.Serialize(new { OrderId = orderId, Total = total, Timestamp = DateTime.UtcNow });
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "payment_exchange", routingKey: "", basicProperties: null, body: body);
    }
}