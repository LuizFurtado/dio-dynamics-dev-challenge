using Microsoft.Xrm.Tooling.Connector;

namespace Demo01_DevDynamics
{
    internal class Conexao
    {
        private static CrmServiceClient client;

        public CrmServiceClient GetClient()
        {
            string connectionStringCRM = @"AuthType=OAuth;
            Username = luiz.furtado@z542m.onmicrosoft.com;
            Password = Esf4R7cQ@2Snr97;SkipDiscovery = True;
            AppId=51f81489-12ee-4a9e-aaae-a2591f45987d;
            RedirectUri=app://58145B91-0C36-4500-8554-080854F2AC97;
            Url = https://org7b6d9363.crm.dynamics.com/main.aspx;";

            if (client == null)
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                client = new CrmServiceClient(connectionStringCRM);
            }

            return client;
        }
    }
}
