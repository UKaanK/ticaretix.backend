using ticaretix.Application;
using ticaretix.Application.Commands;
using ticaretix.backend;
using ticaretix.Core.Interfaces;
using ticaretix.Infrastructure;
using ticaretix.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});
// MediatR'nin tüm handler'larý bulup eklediðinden emin ol


// Diðer baðýmlýlýklar
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationDI(); // MediatR
builder.Services.AddInfrastructureDI(); // Repository
var app = builder.Build();
app.UseCors("AllowAll"); // CORS Middleware'i ekle



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
