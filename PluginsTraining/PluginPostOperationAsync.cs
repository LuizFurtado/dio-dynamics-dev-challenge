using Microsoft.Xrm.Sdk;
using System;

namespace PluginsTraining
{
    public class PluginPostOperationAsync : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            try
            {
                IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
                IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(null);
                ITracingService tracer = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

                if(context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                {
                    Entity contextEntity = (Entity)context.InputParameters["Target"];

                    for(int i = 0; i < 10; i++)
                    {
                        var contact = new Entity("contact");

                        contact.Attributes["firstname"] = "Asyn contact " + i + " created for account";
                        contact.Attributes["lastname"] = contextEntity["name"];
                        contact.Attributes["parentcustomerid"] = new EntityReference("account", contextEntity.Id);
                        contact.Attributes["ownerid"] = new EntityReference("systemuser", context.UserId);

                        tracer.Trace("Creation of contact: " + contact.Attributes["firstname"]);

                        service.Create(contact);
                    }
                }

            } catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
    }
}
