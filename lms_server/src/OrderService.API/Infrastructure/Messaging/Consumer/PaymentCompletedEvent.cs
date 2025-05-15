
using System.Text;
using System.Text.Json;
using OrderService.API.Application.DTOs.Order;
using OrderService.API.Application.Shared.Constant;
using OrderService.API.Application.Shared.Enum;
using OrderService.API.Domain;
using OrderService.API.Infrastructure.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OrderService.API.Infrastructure.Messaging.Consumer;

public class PaymentCompletedEvent : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    public PaymentCompletedEvent(IServiceProvider serviceProvider)
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

        channel.ExchangeDeclare("payment_exchange", ExchangeType.Fanout);
        var queueName = channel.QueueDeclare().QueueName;
        channel.QueueBind(queue: queueName, exchange: "payment_exchange", routingKey: "");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var data = JsonSerializer.Deserialize<PaymentCompletedEventBus>(message);

            if (data == null)
            {
                Console.WriteLine("[OrderService] Failed to deserialize PaymentCompletedEventBus message.");
                return;
            }

            using var scope = _serviceProvider.CreateScope();
            var orderService = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
            var orderItemService = scope.ServiceProvider.GetRequiredService<IOrderItemRepository>();

            var orderReq = new PublishOrderReq(
                OrderId: data.OrderId,
                TotalAmount: data.TotalAmount
            );

            var order = await orderService.GetOrderById(orderReq.OrderId);
            if (order is null)
            {
                Console.WriteLine($"[OrderService] Order not found: {orderReq.OrderId}");
                return;
            }

            var orderItems = await orderItemService.GetAllOrderItems(orderReq.OrderId);
            if (orderItems is null || !orderItems.Any())
            {
                Console.WriteLine($"[OrderService] Order items not found for order: {orderReq.OrderId}");
                return;
            }

            order.Status = OrderStatusEnum.Completed.ToString();
            order.TotalAmount = orderReq.TotalAmount;

            foreach (var orderItem in orderItems)
            {
                orderItem.Status = OrderStatusEnum.Completed.ToString();
                var isUpdated = await orderItemService.UpdateOrderItem(orderItem);
                if (!isUpdated)
                {
                    Console.WriteLine($"[OrderService] Failed to update order item: {orderItem.Id}");
                    return;
                }
            }

            var result = await orderService.UpdateOrder(order);
            if (!result)
            {
                Console.WriteLine($"[OrderService] Failed to update order: {orderReq.OrderId}");
                return;
            }

            var orderObj = new Orders
            {
                UserId = order.UserId,
                TotalAmount = 0,
                Status = OrderStatusEnum.Processing.ToString(),
                CreatedAt = DateTime.UtcNow
            };

            var isCreated = await orderService.CreateOrder(orderObj);
            if (!isCreated)
            {
                Console.WriteLine($"[OrderService] Failed to create order for user: {order.UserId}");
                return;
            }

            Console.WriteLine($"[OrderService] Received payment completed event for order: {data.OrderId}");
        };
        
        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }
}