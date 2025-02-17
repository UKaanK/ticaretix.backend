using Serilog;
using Serilog.Core;
using ticaretix.Infrastructure.Configuration;

namespace ticaretix.Infrastructure.Logging
{
    public static class SerilogRabbitMQLogger
    {
        public static Logger CreateLogger(RabbitMQClientConfiguration rabbitMqConfig)
        {
            return new LoggerConfiguration()
                .WriteTo.RabbitMQ((clientConfig, sinkConfig) =>
                {
                    clientConfig.Username = rabbitMqConfig.Username;
                    clientConfig.Password = rabbitMqConfig.Password;
                    clientConfig.Port = rabbitMqConfig.Port;
                    clientConfig.Hostnames = rabbitMqConfig.Hostnames;
                })
                .CreateLogger();
        }
    }
}
