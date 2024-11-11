using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class TaskConsumer : BackgroundService
{
    public void StartListening()
    {
        var factory = new ConnectionFactory() { HostName = "localhost", DispatchConsumersAsync = true };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.ExchangeDeclare(exchange: "product_reviewed", type: ExchangeType.Fanout);
        var queueName = channel.QueueDeclare().QueueName;
        channel.QueueBind(queue: queueName,
                          exchange: "product_reviewed",
                          routingKey: "");

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            await Task.CompletedTask;
            Console.WriteLine(" [x] Received {0}", message);
        };
        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        Console.WriteLine(" [x] Listenning for product_reviewed");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        StartListening();
        await Task.CompletedTask;
    }

}