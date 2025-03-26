using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ticaretix.Core.Interfaces;

namespace ticaretix.Infrastructure.Logging
{

    public class ElasticsearchLoggerService : ILoggerService
    {
        private readonly HttpClient _httpClient;
        private const string ElasticUrl = "http://localhost:9200/logs/_doc";

        public ElasticsearchLoggerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task LogInfoAsync(string message) => await SendLogAsync("info", message);
        public async Task LogErrorAsync(string message) => await SendLogAsync("error", message);
        public async Task LogWarningAsync(string message) => await SendLogAsync("warning", message);

        private async Task SendLogAsync(string level, string message)
        {
            var logEntry = new
            {
                Timestamp = DateTime.UtcNow,
                Level = level,
                Message = message
            };

            var json = JsonSerializer.Serialize(logEntry);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient.PostAsync(ElasticUrl, content);
        }
    }
}
