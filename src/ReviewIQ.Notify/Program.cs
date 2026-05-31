using Azure.Identity;
using ReviewIQ.Notify;

var builder = Host.CreateApplicationBuilder(args);

// Connect to Azure Key Vault
var keyVaultUrlString = builder.Configuration["KeyVault:Url"];
if (string.IsNullOrWhiteSpace(keyVaultUrlString))
{
    throw new InvalidOperationException("KeyVault: Url configuration value is missing or empty.");
}
var keyVaultUrl = new Uri(keyVaultUrlString);
builder.Configuration.AddAzureKeyVault(keyVaultUrl, new DefaultAzureCredential());

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
