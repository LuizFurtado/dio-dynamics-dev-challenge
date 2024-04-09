using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;

namespace Demo01_DevDynamics
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var crmService = new Conexao().GetClient();
            FetchXML(crmService);
            Create(crmService);
        }

        static void FetchXML(CrmServiceClient client)
        {
            string query = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                <entity name='account'>
                    <attribute name='name' />
                    <attribute name='telephone1' />
                    <attribute name='accountid' />
                    <order attribute='name' descending='false' />
                </entity>
            </fetch>";

            EntityCollection results = client.RetrieveMultiple(new FetchExpression(query));

            foreach (var result in results.Entities)
            {
                Console.WriteLine(result.Attributes["name"]);
                if (result.Attributes.Contains("telephone1"))
                {
                    Console.WriteLine(result["telephone1"]);
                }
            }

            Console.ReadLine();
        }

        static void Create(CrmServiceClient client)
        {
            Entity account = new Entity("account");
            account["name"] = "Training Dev Dynamics";
            account["telephone1"] = "123456789";
            Guid newAccount = client.Create(account);

            if(newAccount != Guid.Empty)
            {
                Console.WriteLine("Account created with id: " + newAccount);
            } else
            {
                Console.WriteLine("Account not created");
            }

            Console.ReadLine();
        }

        static void Update(CrmServiceClient client, Guid accountId)
        {
            Entity account = new Entity("account", accountId);
            account["name"] = "Training Dev Dynamics - Updated";
            account["telephone1"] = "+1 123456789";
            client.Update(account);
        }

        static void Delete(CrmServiceClient client, Guid accountId)
        {
            Entity account = client.Retrieve("account", accountId, new ColumnSet("accountid"));
            if (account.Attributes.Count > 0)
            {
                client.Delete(account.LogicalName, accountId);
                Console.WriteLine("Account deleted with id: " + accountId);
                Console.ReadLine();
            }
        }
    }
}
