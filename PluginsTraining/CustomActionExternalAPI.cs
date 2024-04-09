using Microsoft.Xrm.Sdk;
using System;
using System.Net;
using System.Text;

namespace PluginsTraining
{
    public class CustomActionExternalAPI : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(null);
            ITracingService tracer = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            var cep = context.InputParameters["CepInput"];

            tracer.Trace("Cep: " + cep.ToString());

            var viaCepUrl = $"http://viacep.com.br/ws/{cep}/json/";
            string result = string.Empty;

            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Encoding = Encoding.UTF8;
                result = client.DownloadString(viaCepUrl);
            }

            context.OutputParameters["CepOutput"] = result;

            tracer.Trace("Result: " + result);
        }
    }
}
