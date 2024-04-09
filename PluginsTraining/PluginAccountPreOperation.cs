using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace PluginsTraining
{
    public class PluginAccountPreOperation : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(null);
            ITracingService tracer = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                Entity contextEntity = (Entity)context.InputParameters["Target"];

                if(contextEntity.LogicalName == "account")
                {
                    if(contextEntity.Contains("telephone1"))
                    {
                        var phone1 = contextEntity["telephone1"].ToString();

                        string fetchContact = @"<?xml version='1.0'?>" + 
                            "<fetch distinct='false' mapping='logical' output-format='xml-platform' version='1.0'>" +
                            " <entity name='contact'>" +
                            " <attribute name='fullname' />" +
                            " <attribute name='telephone1' />" +
                            "< attrbutute name='contactid' />" +
                            "<order descending='false' attribute='fullname' />" +
                            "<filter type='and'>" +
                            "condition attribute='telephone1' operator='eq' value='" + phone1 + "' />" +
                            "</filter>" +
                            "</entity>" +
                            "</fetch>";
                        
                        tracer.Trace("Fetch Contact: " + fetchContact);

                        var primaryContact = service.RetrieveMultiple(new FetchExpression(fetchContact));

                        if(primaryContact.Entities.Count > 0)
                        {
                            foreach(var contact in primaryContact.Entities)
                            {
                                contextEntity["primarycontactid"] = new EntityReference("contact", contact.Id);
                            }
                        }
                    }
                }
            }
        }
    }
}
