using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;

var builder = WebApplication.CreateBuilder(args);

// Connect to Azure Key Vault
var keyVaultUrlString = builder.Configuration["KeyVault:Url"];
if (string.IsNullOrWhiteSpace(keyVaultUrlString))
{
    throw new InvalidOperationException("KeyVault: Url configuration value is missing or empty.");
}
var keyVaultUrl = new Uri(keyVaultUrlString);
builder.Configuration.AddAzureKeyVault(keyVaultUrl, new DefaultAzureCredential());

// Add services
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();