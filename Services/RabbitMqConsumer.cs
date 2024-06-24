using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class RabbitMQConsumer
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _queueName;

    public RabbitMQConsumer(string hostName, string queueName)
    {
        var factory = new ConnectionFactory() { HostName = hostName };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _queueName = queueName;

        _channel.QueueDeclare(queue: _queueName,
                              durable: false,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);
    }

    public void Consume()
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Received message: {message}");

            // Add your message processing logic here

            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume(queue: _queueName,
                              autoAck: false,
                              consumer: consumer);

        // Console.WriteLine("Consumer started. Press [Enter] to exit.");
        // Console.ReadLine();
    }

    public void Close()
    {
        _connection.Close();
    }
}
