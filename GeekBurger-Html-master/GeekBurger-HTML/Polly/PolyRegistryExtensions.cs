using System;
using System.Net.Http;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Registry;

namespace GeekBurger_HTML.Controllers
{
    public static class PolyRegistryExtensions
    {
        public static IPolicyRegistry<string> AddBasicRetryPolicy(this IPolicyRegistry<string> policyRegistry)
        {
            var retryPolicy = Policy
                .Handle<Exception>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(3, retryCount => TimeSpan.FromSeconds(Math.Pow(2, retryCount)), (result, timeSpan, retryCount, context) =>
                {
                    if (!context.TryGetLogger(out var logger)) return;

                    context.TryGetValue("url", out var url);

                    var telemetry = new TelemetryClient();
                    if (result.Exception != null)
                    {
                        telemetry.TrackException(new Exception($"An exception occurred on retry {retryCount} for {context.PolicyKey} " +
                                                               $"on URL {url}"));
                    }
                    else
                    {
                        telemetry.TrackException(new Exception($"A non success code {(int)result.Result.StatusCode} " +
                                                               $"was received on retry {retryCount} for {context.PolicyKey}. " +
                                                               $"Will retry in {Math.Pow(2, retryCount)} seconds on URL {url}"));
                    }
                })
                .WithPolicyKey(PolicyNames.BasicRetry);

            policyRegistry.Add(PolicyNames.BasicRetry, retryPolicy);

            return policyRegistry;
        }
    }
}