using ReviewIQ.Gateway.Domain;

namespace ReviewIQ.Gateway.Interfaces
{
    public interface IHmacValidationService
    {
        bool IsValid(string signatureHeader, string rawBody);
    }
}
