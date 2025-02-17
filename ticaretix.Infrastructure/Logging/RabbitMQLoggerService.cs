using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ticaretix.Infrastructure.Logging
{
    public class RabbitMQLoggerService
    {
        private readonly string _hostname = "localhost"; // RabbitMQ Sunucu Adı
        private readonly string _queueName = "error_logs"; // Kuyruk Adı

        public async Task LogErrorAsync(string message)
        {
            try
            {
                // RabbitMQ bağlantı fabrikası oluştur
                var factory = new ConnectionFactory() { HostName = _hostname };

                // Bağlantıyı asenkron olarak oluştur
                await using var connection = await factory.CreateConnectionAsync();
                await using var channel = await connection.CreateChannelAsync();

                // Kuyruğu oluştur (eğer yoksa)
                await channel.QueueDeclareAsync(
                    queue: _queueName,
                    durable: true,        // Kuyruk sunucu restartlarında hayatta kalır
                    exclusive: false,     // Kuyruk diğer bağlantılara açık olur
                    autoDelete: false,    // Kuyruk otomatik silinmez
                    arguments: null);    // Ekstra argümanlar eklemiyoruz

                // Mesajı JSON formatında kuyruğa göndermek için byte dizisine dönüştür
                var body = Encoding.UTF8.GetBytes(message);


                // Mesajı kuyruğa asenkron olarak yayınla
                await channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: _queueName,
                    mandatory: false,
                    body: body);

                Console.WriteLine($"[📩] Log Mesajı Gönderildi: {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] RabbitMQ'ya log gönderme hatası: {ex.Message}");
            }
        }
    }
}
