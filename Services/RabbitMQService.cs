using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;

public class RabbitMQService : IMessageQueueService
{
    private readonly IConnectionFactory _connectionFactory;

    public RabbitMQService(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task PublishEmailNotificationAsync(string userEmail, string message)
    {
        using (var connection = _connectionFactory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "email_queue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes($"To: {userEmail}\n\n{message}");

            channel.BasicPublish(exchange: "",
                                 routingKey: "email_queue",
                                 basicProperties: null,
                                 body: body);
        }

        await Task.CompletedTask;
    }
}
