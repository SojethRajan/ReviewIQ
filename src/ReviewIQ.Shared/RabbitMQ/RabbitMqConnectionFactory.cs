using RabbitMQ.Client;

namespace ReviewIQ.Shared.RabbitMQ
{
    public static class RabbitMqConnectionFactory
    {
        public static async Task<IConnection> CreateConnectionAsync(string hostName, string userName, string password)
        {
            var factory = new ConnectionFactory
            {
                HostName = hostName,
                UserName = userName,
                Password = password,
                AutomaticRecoveryEnabled = true
            };
            return await factory.CreateConnectionAsync();
        }
    }
}
