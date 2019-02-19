using Microsoft.Extensions.DependencyInjection;

namespace GeekBurger_HTML.Controllers
{
    public static class PollyServiceCollectionExtensions
    {
        public static IServiceCollection AddPollyPolicies(this IServiceCollection services)
        {
            var registry = services.AddPolicyRegistry();

            registry.AddBasicRetryPolicy();

            return services;
        }
    }
}