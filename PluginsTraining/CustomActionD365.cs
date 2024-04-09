using Microsoft.Xrm.Sdk;
using System;

namespace PluginsTraining
{
    public class CustomActionD365 : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(null);
            ITracingService tracer = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            tracer.Trace("My custom action was successfully executed.");

            Entity lead = new Entity("lead");
            lead["firstname"] = "Lead";
            lead["lastname"] = "Custom Action";
            lead["subject"] = "This is a Lead created from custom action.";
            lead["mobilephone"] = "555-555-5555";
            lead["ownerid"] = new EntityReference("systemuser", context.UserId);

            Guid leadId = service.Create(lead);

            tracer.Trace("Create lead with ID: " + leadId);
        }
    }
}
