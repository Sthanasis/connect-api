using System.Text;
using RabbitMQ.Client;

namespace connect.Services;
public class TaskPublisher
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public TaskPublisher()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: "product_updated", durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    public void PublishMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: "", routingKey: "product_updated", basicProperties: null, body: body);
        Console.WriteLine(" [x] Sent {0}", message);
    }

    public void Close()
    {
        _channel.Close();
        _connection.Close();
    }
}