using System.Diagnostics;
using System.Text;
using System.Text.Json;
using connect.Models;
using connect.Services;
using connect_utilities.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public sealed class TaskConsumer(ProductService productService) : BackgroundService
{
    private IConnection? connection;
    private IModel? channel;
    private readonly ProductService _productService = productService;
    public void StartListening()
    {
        var factory = new ConnectionFactory() { HostName = "localhost", DispatchConsumersAsync = true };
        connection = factory.CreateConnection();
        channel = connection.CreateModel();
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
            var review = JsonSerializer.Deserialize<ReviewMessage>(message);
            if (review != null) await _productService.UpdateProductRating(review);
            Console.WriteLine(" [x] Received {0}", review);
        };
        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        Console.WriteLine(" [x] Listenning for product_reviewed");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            StartListening();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        finally
        {
            await Task.CompletedTask;
        }

    }

}