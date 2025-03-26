
using Elastic.CommonSchema;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.RabbitMQ;
using StackExchange.Redis;
using System.Text;
using ticaretix.Application;
using ticaretix.Application.Interfaces;
using ticaretix.Application.Services;
using ticaretix.Application.UseCases;
using ticaretix.Core.Entities;
using ticaretix.Infrastructure.Repositories;
using ticaretix.Core.Interfaces;
using ticaretix.Infrastructure;
using ticaretix.Infrastructure.Data;
using ticaretix.Infrastructure.Logging;
using ticaretix.Infrastructure.Middlewares;
using ticaretix.Infrastructure.Redis; // Middleware'i dahil ettik


var builder = WebApplication.CreateBuilder(args);
const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=ticaretix;Trusted_Connection=True;";
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(connectionString));
// RabbitMQ Ayarları
var rabbitMqConfig = new RabbitMQClientConfiguration
{
    Username = "guest",
    Password = "guest",
    Port = 5672,
    Hostnames = new List<string> { "localhost" }
};

// Serilog RabbitMQ Konfigürasyonu
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.RabbitMQ((clientConfig, sinkConfig) =>
    {
        clientConfig.Username = rabbitMqConfig.Username;
        clientConfig.Password = rabbitMqConfig.Password;
        clientConfig.Port = rabbitMqConfig.Port;
        clientConfig.Hostnames = rabbitMqConfig.Hostnames;
    })
    // Elasticsearch'e log yazmak için Serilog yapılandırması
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200")) // Elasticsearch URI
    {
        IndexFormat = "logs-{0:yyyy.MM.dd}", // Günlük loglar için indeks formatı
        AutoRegisterTemplate = true, // Elasticsearch şablonlarını otomatik kaydetme
        FailureCallback = e =>
        {
            // Hata durumu callback
            Console.WriteLine("Elasticsearch log yazma hatası: " + e);
        },
       
    })
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);




// RabbitMQLoggerService'i DI Container'a ekleyin
builder.Services.AddSingleton<RabbitMQLoggerService>();
// Get the JwtSettings section
var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");

// Create a JwtSettings object and populate it from the section
var jwtSettings = new JwtSettings();
jwtSettingsSection.Bind(jwtSettings);

// Configure using the populated object
builder.Services.Configure<JwtSettings>(options =>
{
    options.Secret = jwtSettings.Secret;
    options.Issuer = jwtSettings.Issuer;
    options.Audience = jwtSettings.Audience;
    options.ExpiryMinutes = jwtSettings.ExpiryMinutes; // If you have this property
});
// Bağlantı stringini burada doğrudan tanımla

builder.Services.AddHttpClient<ILoggerService, ElasticsearchLoggerService>();
builder.Services.AddScoped<IUrunlerRepository, UrunlerRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IKullaniciRepository, KullaniciRepository>();
builder.Services.AddScoped<ISepetDetaylarıRepository, SepetDetayRepository>();
builder.Services.AddScoped<ISepetRepository, SepetRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<LoginUseCase>();
builder.Services.AddScoped<RefreshTokenUseCase>();
//services.AddTransient<ExceptionMiddleware>(); // Transient olarak kaydedin

builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:6379"));
builder.Services.AddSingleton<IJwtDecoderService, JwtDecoderService>();


builder.Services.AddSingleton<IRedisService, RedisService>();  // Redis servisini ekleyin
                                                       // Redis'i yapılandırma

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});
// MediatR'nin tüm handler'ları bulup eklediğinden emin ol


// Diğer bağımlılıklar
// JWT doğrulaması için gerekli ayarları yapıyoruz
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]))
        };
    });
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationDI(); // MediatR
//builder.Services.AddInfrastructureDI(builder.Configuration); // Repository


var app = builder.Build();
app.UseCors("AllowAll"); // CORS Middleware'i ekle
app.UseRouting();
app.MapGet("/", () =>
{
    Serilog.Log.Information("Anasayfa çağrıldı.");
    return "Merhaba, Loglama Başladı!";
});



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
