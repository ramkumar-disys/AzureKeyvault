using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace keyvaultsamplecode2
{
    class Program
    {
        
        const string CLIENTSECRET = "0N.PY452cvMPgXsR_VK~r68_gGb1-~M.Nq";
        const string CLIENTID = "4b469954-deb3-464d-b89e-a67d26be390c";
        const string BASESECRETURI = "https://rb-keyvault-dev.vault.azure.net";
        const string SECRETNAME = "secret1";

        static KeyVaultClient kvc = null;
        static void Main(string[] args)
        {
            DoVault();

            Console.ReadLine();
        }
        public static async Task<string> GetToken(string authority, string resource, string scope)
        {
            var authContext = new AuthenticationContext(authority);
            ClientCredential clientCred = new ClientCredential(CLIENTID, CLIENTSECRET);
            AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientCred);

            if (result == null)
                throw new InvalidOperationException("Failed to obtain the JWT token");

            return result.AccessToken;
        }
        private static void DoVault()
        {
            kvc = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetToken));

            // write
           // writeKeyVault();
           

            SecretBundle secret = Task.Run(() => kvc.GetSecretAsync(BASESECRETURI +
                @"/secrets/" + SECRETNAME)).ConfigureAwait(false).GetAwaiter().GetResult();
            Console.WriteLine(secret.Value);
            //Console.WriteLine(secret.Tags["Test1"].ToString());


            Console.ReadLine();

        }

        private static async void writeKeyVault()
        {
            SecretAttributes attribs = new SecretAttributes
            {
                Enabled = true
            };

            IDictionary<string, string> alltags = new Dictionary<string, string>();
            alltags.Add("Test1", "This is a test1 value");
           
            alltags.Add("CanBeAnything", "Including a long encrypted string if you choose");
            string TestName = "TestSecret";
            string TestValue = "searchValue"; 
            string contentType = "SecretInfo"; 

            SecretBundle bundle = await kvc.SetSecretAsync
               (BASESECRETURI, TestName, TestValue, alltags, contentType, attribs);
            Console.WriteLine("Bundle:" + bundle.Tags["Test1"].ToString());
        }
    }
}
