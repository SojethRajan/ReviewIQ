using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using ReviewIQ.AI.AIProvider.Interfaces;
using ReviewIQ.AI.AIProvider.Services;
using ReviewIQ.AI.Infrastructure;
using ReviewIQ.AI.Interfaces;
using ReviewIQ.AI.Services;
using ReviewIQ.AI.Workers;
using ReviewIQ.Shared.RabbitMQ;

var builder = Host.CreateApplicationBuilder(args);

// Connect to Azure Key Vault
var keyVaultUrlString = builder.Configuration["KeyVault:Url"];
if (string.IsNullOrWhiteSpace(keyVaultUrlString))
{
    throw new InvalidOperationException("KeyVault: Url configuration value is missing or empty.");
}
var keyVaultUrl = new Uri(keyVaultUrlString);
builder.Configuration.AddAzureKeyVault(keyVaultUrl, new DefaultAzureCredential());


//database connection
builder.Services.AddDbContext<AiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ReviewIQDb")));
    
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

// RabbitMQ topology
builder.Services.AddSingleton<QueueDeclarationService>();

// GitHub HttpClient
builder.Services.AddHttpClient<IDiffFetcherService, DiffFetcherService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["GitHub:BaseUrl"]!);
    client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
    client.DefaultRequestHeaders.Add("User-Agent", "ReviewIQ");
    client.DefaultRequestHeaders.Add("Authorization",
        $"Bearer {builder.Configuration["GitHub:Token"]}");
});


// Gemini HttpClient
builder.Services.AddHttpClient<IGeminiService, GeminiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Gemini:BaseUrl"]!);
});

// Services
builder.Services.AddScoped<IDiffChunker, DiffChunker>();
builder.Services.AddScoped<IReviewOrchestrator, ReviewOrchestrator>();
builder.Services.AddScoped<IReviewPublisher, ReviewPublisher>();

//worker
builder.Services.AddHostedService<PrReviewWorker>();

var host = builder.Build();

// Declare RabbitMQ topology on startup
using (var scope = host.Services.CreateScope())
{
    var queueDeclaration = scope.ServiceProvider
        .GetRequiredService<QueueDeclarationService>();
    await queueDeclaration.DeclareAllAsync();
}

host.Run();
