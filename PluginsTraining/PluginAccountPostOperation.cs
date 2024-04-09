using Microsoft.Xrm.Sdk;
using System;

namespace PluginsTraining
{
    public class PluginAccountPostOperation : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            try
            {
                IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
                IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(null);
                ITracingService tracer = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                {
                    Entity contextEntity = (Entity)context.InputParameters["Target"];
                    if (!contextEntity.Contains("telephone1"))
                    {
                        throw new InvalidPluginExecutionException("Telephone is a required field.");
                    }

                    if(!contextEntity.Contains("websiteurl"))
                    {
                        throw new InvalidPluginExecutionException("Website URL is a required field.");
                    }

                    var task = new Entity("task");

                    task.Attributes["ownerid"] = new EntityReference("systemuser", context.UserId);
                    task.Attributes["regardingobjectid"] = new EntityReference("account", contextEntity.Id);
                    task.Attributes["subject"] = "Visit customer site: " + contextEntity["websiteurl"];
                    task.Attributes["description"] = "Task created from the plugin post-operation.";

                    service.Create(task);
                }
            }
            catch(Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
    }
}
