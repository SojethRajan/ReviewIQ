using ReviewIQ.Gateway.Domain;
using ReviewIQ.Gateway.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace ReviewIQ.Gateway.Services
{
    public class HmacValidationService : IHmacValidationService
    {
        private readonly string _secret;

        public HmacValidationService(IConfiguration configuration)
        {
            _secret = configuration["GitHub:WebhookSecret"] ?? throw new ArgumentNullException("GitHub:WebhookSecret configuration is missing.");
        }

        public bool IsValid(string signatureHeader, string rawBody)
        {
            var signature = GitHubSignature.Parse(signatureHeader);
            var secretBytes = Encoding.UTF8.GetBytes(_secret);
            var rawBodyBytes = Encoding.UTF8.GetBytes(rawBody);

            using var hmac = new HMACSHA256(secretBytes);
            var computedHash = hmac.ComputeHash(rawBodyBytes);

            return CryptographicOperations.FixedTimeEquals(computedHash, Convert.FromHexString(signature.Hash.ToLower()));
        }
    }
}
