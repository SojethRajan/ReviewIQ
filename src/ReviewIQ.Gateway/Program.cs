using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;

var builder = WebApplication.CreateBuilder(args);

// Connect to Azure Key Vault
var keyVaultUrlString = builder.Configuration["KeyVault:Url"];
if (string.IsNullOrWhiteSpace(keyVaultUrlString))
{
    var keyVaultUrl = new Uri(keyVaultUrlString);
    builder.Configuration.AddAzureKeyVault(keyVaultUrl, new DefaultAzureCredential());  
}

// Add services
builder.Services.AddControllers();
builder.Services.AddOpenApi();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.Run();