using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Controllers
{
    public class ApiHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            //https://www.rabbitmq.com/monitoring.html - Node Metrics
            //https://www.rabbitmq.com/management.html
            //https://rawcdn.githack.com/rabbitmq/rabbitmq-management/v3.8.2/priv/www/api/index.html

            //TODO: Implement your own healthcheck logic here
            var isHealthy = true;
            if (isHealthy)
            {
                return Task.FromResult(HealthCheckResult.Healthy("I am one healthy microservice API"));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("I am the sad, unhealthy microservice API"));
        }
    }
}
