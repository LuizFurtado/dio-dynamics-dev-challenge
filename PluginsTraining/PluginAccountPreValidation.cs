using Microsoft.Xrm.Sdk;
using System;

namespace PluginsTraining
{
    public class PluginAccountPreValidation : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(null);
            ITracingService tracer = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            Entity contextEntity = null;

            if (context.InputParameters.Contains("Target"))
            {
                contextEntity = (Entity)context.InputParameters["Target"];
                tracer.Trace("Account Pre Validation: " + contextEntity.Attributes.Count);

                if(contextEntity == null)
                {
                    return;
                }

                if(!contextEntity.Contains("telephone"))
                {
                    throw new InvalidPluginExecutionException("Telephone is a required field");

                }
            }
        }
    }
}
