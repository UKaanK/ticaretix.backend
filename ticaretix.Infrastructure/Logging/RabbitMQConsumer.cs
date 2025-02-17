using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Nest;  // Elasticsearch için

namespace ticaretix.Infrastructure.Logging
{
    public class RabbitMQConsumer
    {
        private readonly string _hostname = "localhost";
        private readonly string _queueName = "error_logs";
        private readonly ElasticClient _elasticClient;

        public RabbitMQConsumer()
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
                .DefaultIndex("logs");

            _elasticClient = new ElasticClient(settings);
            CreateIndexMapping();
            TestElasticsearchConnection();
        }

        private void CreateIndexMapping()
        {
            var createIndexResponse = _elasticClient.Indices.Create("logs", c => c
                .Map<LogMessage>(m => m
                    .AutoMap()
                    .Properties(p => p
                        .Date(d => d.Name(n => n.Timestamp))
                        .Text(t => t.Name(n => n.Message))
                        .Text(t => t.Name(n => n.Detailed))
                        .Number(n => n.Name(nn => nn.StatusCode).Type(NumberType.Integer))
                    )
                )
            );

            if (createIndexResponse.IsValid)
            {
                Console.WriteLine("✅ Index mapping başarıyla oluşturuldu.");
            }
            else
            {
                Console.WriteLine($"❌ Index mapping hatası: {createIndexResponse.OriginalException?.Message}");
            }
        }

        // Elasticsearch bağlantısını test eder
        private void TestElasticsearchConnection()
        {
            var pingResponse = _elasticClient.Ping();
            if (!pingResponse.IsValid)
            {
                Console.WriteLine("❌ Elasticsearch'e bağlantı sağlanamadı.");
                Console.WriteLine($"Hata: {pingResponse.OriginalException?.Message}");
            }
            else
            {
                Console.WriteLine("✅ Elasticsearch'e bağlantı başarılı.");
            }
        }

        public async Task StartConsumingAsync()
        {
            var factory = new ConnectionFactory() { HostName = _hostname };
            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"[🐇] RabbitMQ'den Gelen Mesaj: {message}");

                    var logMessage = JsonSerializer.Deserialize<LogMessage>(message);
                    Console.WriteLine($"[📤] Elasticsearch'e Gönderilen Veri: {JsonSerializer.Serialize(logMessage)}");

                    var response = await _elasticClient.IndexDocumentAsync(logMessage);
                    if (response.IsValid)
                    {
                        Console.WriteLine("✅ Log başarıyla Elasticsearch'e kaydedildi.");
                    }
                    else
                    {
                        Console.WriteLine($"❌ Elasticsearch Hatası: {response.OriginalException?.Message}");
                        Console.WriteLine($"🔍 Debug Info: {response.DebugInformation}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[❌] Log işlenirken hata oluştu: {ex.Message}");
                }
            };

            await channel.BasicConsumeAsync(queue: _queueName, autoAck: true, consumer: consumer);
            Console.WriteLine("RabbitMQ Consumer çalışıyor... Çıkış için CTRL+C");
            Console.ReadLine();
        }

        private class LogMessage
        {
            public int StatusCode { get; set; }
            public string Message { get; set; }
            public string Detailed { get; set; }
            public DateTime Timestamp { get; set; }
        }
    }
}
