using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Extensions.Configuration;

namespace Microservices.UI.Services.Extensions
{
    public static class ServiceBusNamespaceExtension
    {
        public static IServiceBusNamespace GetServiceBusNamespace(this IConfiguration configuration)
        {
            var sbConfig = configuration.GetSection("serviceBus").Get<ServiceBusConfiguration>();

            var credentials = SdkContext.AzureCredentialsFactory
                .FromServicePrincipal(sbConfig.ClientId, sbConfig.ClientSecret,
                    sbConfig.TenantId, AzureEnvironment.AzureGlobalCloud);

            var serviceBusManager = ServiceBusManager.Authenticate(credentials, sbConfig.SubscriptionId);
            return serviceBusManager.Namespaces.GetByResourceGroup(sbConfig.ResourceGroup, sbConfig.NamespaceName);
        }
    }
}