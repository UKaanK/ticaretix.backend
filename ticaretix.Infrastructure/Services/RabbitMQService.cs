using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ticaretix.Infrastructure.Services
{
    public class RabbitMQService : IDisposable
    {
        private readonly ConnectionFactory _factory;
        private IConnection _connection;
        private IChannel _channel;
        private readonly string _queueName = "test-queue";

        public RabbitMQService()
        {
            _factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
        }

        public async Task ConnectAsync()
        {
            _connection = await _factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            await _channel.QueueDeclareAsync(_queueName, durable: true, exclusive: false, autoDelete: false);
        }

        public async Task PublishAsync(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            await _channel.BasicPublishAsync(exchange: "", routingKey: _queueName, body: body);
        }

        public async Task ConsumeAsync()
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"📩 Mesaj alındı: {message}");
                await Task.Yield();
            };

            await _channel.BasicConsumeAsync(queue: _queueName, autoAck: true, consumer: consumer);
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
