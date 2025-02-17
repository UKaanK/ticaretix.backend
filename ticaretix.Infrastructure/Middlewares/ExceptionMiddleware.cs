using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;
using ticaretix.Core.Exceptions;
using ticaretix.Infrastructure.Logging; // RabbitMQLoggerService'yi içerir

namespace ticaretix.Infrastructure.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly RabbitMQLoggerService _rabbitLogger;

        // RabbitMQLoggerService, DI ile enjekte ediliyor
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, RabbitMQLoggerService rabbitLogger)
        {
            _next = next;
            _logger = logger;
            _rabbitLogger = rabbitLogger;
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception, Microsoft.Extensions.Logging.ILogger logger, RabbitMQLoggerService rabbitLogger)
        {
            context.Response.ContentType = "application/json";
            // Varsayılan olarak 500 hata kodu
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            string errorMessage;

            if (exception is ApiException apiException)
            {
                // Hata kodunu uygun HTTP durum koduna eşle
                context.Response.StatusCode = apiException.ErrorCode switch
                {
                    ErrorCodes.U101 => StatusCodes.Status404NotFound,
                    ErrorCodes.U102 => StatusCodes.Status401Unauthorized,
                    ErrorCodes.S500=> StatusCodes.Status500InternalServerError,
                    ErrorCodes.S502 =>StatusCodes.Status200OK,
                    _ => StatusCodes.Status500InternalServerError
                };

                errorMessage = JsonSerializer.Serialize(new
                {
                    StatusCode = apiException.ErrorCode,
                    Message = apiException.ErrorMessage,
                    Timestamp = DateTime.UtcNow
                });
            }
            else
            {
                errorMessage = JsonSerializer.Serialize(new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Bilinmeyen bir hata oluştu",
                    Detailed = exception.Message,
                    Timestamp = DateTime.UtcNow
                });
            }

            // Hata logunu RabbitMQ'ya gönder (await kullanılıyor, çünkü metod asenkron)
            await rabbitLogger.LogErrorAsync(errorMessage);
            logger.LogError(exception, "Hata yakalandı: {ErrorMessage}", exception.Message);
            // Elasticsearch'e de log gönderme
            Log.Error($"Hata: {exception.Message} - {errorMessage}");
            await context.Response.WriteAsync(errorMessage);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // İstekleri sonraki middleware'e aktar
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _logger, _rabbitLogger);
            }
        }
    }
}
