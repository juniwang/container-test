using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace JWTest
{
    internal class KeyVaultTestClient
    {
        public async Task Test()
        {
            var uri = new Uri(Environment.GetEnvironmentVariable("kvBasUri"));
            var client = new SecretClient(uri, GetCredential());

            while (true)
            {
                try
                {
                    var secret = await client.GetSecretAsync("test");
                    await Console.Out.WriteLineAsync("CurrentValue: " + secret.Value.Value);
                    await client.SetSecretAsync("test", DateTime.UtcNow.ToLongTimeString());
                }
                catch (Exception e)
                {
                    await Console.Out.WriteLineAsync(e.Message);
                }
                await Task.Delay(1000);
            }
        }

        private TokenCredential GetCredential()
        {
            var clientId = Environment.GetEnvironmentVariable("ClientId");
            var options = new WorkloadIdentityCredentialOptions
            {
                ClientId = clientId
            };
            return new WorkloadIdentityCredential(options);
        }
    }
}
