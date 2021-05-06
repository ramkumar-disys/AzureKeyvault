using System;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;



namespace KeyvaultSampleCode
{
    class Program
    {

       
        static void Main(string[] args)
        {
            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
                {
                    Delay= TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                 }
            };
            var client = new SecretClient(new Uri("https://rb-keyvault-dev.vault.azure.net"), new DefaultAzureCredential(), options);

            KeyVaultSecret secret = client.GetSecret("secret1");

            string secretValue = secret.Value;


        }
    }
}
