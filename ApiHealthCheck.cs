using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace api
{
    public class ApiHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var healthCheckResultHealthy = true;

            if (healthCheckResultHealthy)
            {
                return Task.FromResult(HealthCheckResult.Healthy());
            }

            // TODO: add code here to check any dependent services - if one or more fails but doesn't
            // result in an unhealthy service you can return 
            // and be sure to add some information that helps the operator to identify where the problem actually is. "Something went wrong" is not at all helpful!
            // HealthCheckResult.Degraded();
            // See: https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-3.1 for some examples of additional probes for SQL and Cosmos.

            return Task.FromResult(HealthCheckResult.Unhealthy("My model API is unhealthy, please investigate."));
        }
    }
}
