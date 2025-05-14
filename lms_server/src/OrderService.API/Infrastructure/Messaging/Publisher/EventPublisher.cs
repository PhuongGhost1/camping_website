// using System.Text;
// using System.Text.Json;
// using AuthenticationService.API.Application.Shared.Constant;
// using RabbitMQ.Client;

// namespace AuthenticationService.API.Infrastructure.Messaging.Publisher;

// public interface IEventPublisher
// {
//     void PublishUserLoggedIn(Guid userId);
// }
// public class EventPublisher : IEventPublisher
// {
//     private readonly IConnectionFactory _connectionFactory;
//     public EventPublisher()
//     {
//         _connectionFactory = new ConnectionFactory()
//         {
//             HostName = RabbitMQConst.RABBITMQ_HOST,
//             Port = int.Parse(RabbitMQConst.RABBITMQ_PORT),
//             UserName = RabbitMQConst.RABBITMQ_USERNAME,
//             Password = RabbitMQConst.RABBITMQ_PASSWORD
//         };
//     }
//     public void PublishUserLoggedIn(Guid userId)
//     {
//         using var connection = _connectionFactory.CreateConnection();
//         using var channel = connection.CreateModel();

//         channel.ExchangeDeclare(exchange: "auth_exchange", type: ExchangeType.Fanout);

//         var message = JsonSerializer.Serialize(new { UserId = userId, Timestamp = DateTime.UtcNow });
//         var body = Encoding.UTF8.GetBytes(message);

//         channel.BasicPublish(exchange: "auth_exchange", routingKey: "", basicProperties: null, body: body);
//     }
// }