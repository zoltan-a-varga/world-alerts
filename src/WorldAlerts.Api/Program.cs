using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using WorldAlerts.Core.Alerts;
using WorldAlerts.Infrastructure.Persistence;
using WorldAlerts.Core.Events;
using WorldAlerts.Core.Notifications;
using WorldAlerts.Infrastructure.Notifications;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "World Alerts API",
        Version = "v1"
    });
});

builder.Services.AddDbContext<AlertsDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("WorldAlerts")));

builder.Services.AddScoped<IAlertRepository, AlertRepository>();
builder.Services.AddSingleton<AlertMatcher>();
builder.Services.AddScoped<EventProcessingService>();
builder.Services.AddScoped<INotificationChannel, EmailNotificationChannel>();
builder.Services.AddScoped<INotificationChannel, SlackNotificationChannel>();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(
        "/swagger/v1/swagger.json",
        "World Alerts API v1");
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();