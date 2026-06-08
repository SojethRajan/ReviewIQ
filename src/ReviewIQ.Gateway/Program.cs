using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using ReviewIQ.Gateway.Infrastructure;
using ReviewIQ.Gateway.Interfaces;
using ReviewIQ.Gateway.Services;
using ReviewIQ.Shared.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Connect to Azure Key Vault
var keyVaultUrlString = builder.Configuration["KeyVault:Url"];
if (!string.IsNullOrWhiteSpace(keyVaultUrlString))
{
    var keyVaultUrl = new Uri(keyVaultUrlString);
    builder.Configuration.AddAzureKeyVault(keyVaultUrl, new DefaultAzureCredential());
}

// Database 
builder.Services.AddDbContext<GatewayDbContext>(options =>
    options.UseSqlServer(builder.Configuration
        .GetConnectionString("ReviewIQDb")));


// RabbitMQ
builder.Services.AddSingleton<IConnection>(sp =>
{
    var host = builder.Configuration["RabbitMQ:Host"]!;
    var username = builder.Configuration["RabbitMQ:Username"]!;
    var password = builder.Configuration["RabbitMQ:Password"]!;

    return RabbitMqConnectionFactory
        .CreateConnectionAsync(host, username, password)
        .GetAwaiter()
        .GetResult();
});

// Add services
builder.Services.AddSingleton<QueueDeclarationService>();

// --- Gateway Services ---
builder.Services.AddScoped<IHmacValidationService, HmacValidationService>();
builder.Services.AddScoped<IGitHubPayloadParser, GitHubPayloadParser>();
builder.Services.AddScoped<IWebhookPublisher, WebhookPublisher>();
builder.Services.AddScoped<IWebhookOrchestrator, WebhookOrchestrator>();


builder.Services.AddControllers();

var app = builder.Build();

// Declare RabbitMQ topology on startup 
using (var scope = app.Services.CreateScope())
{
    var queueDeclaration = scope.ServiceProvider
        .GetRequiredService<QueueDeclarationService>();
    await queueDeclaration.DeclareAllAsync();
}

app.MapControllers();

app.Run();