using Microsoft.Extensions.Logging;
using Quartz;

namespace simple_quartz_job;

public class HealthyCheckJob : IJob
{
    private readonly ILogger<HealthyCheckJob> _logger;

    public HealthyCheckJob(ILogger<HealthyCheckJob> logger)
    {
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation(
            $"[{DateTime.UtcNow.ToString("u")}][{context.Trigger.JobKey}] Healthy.");

        await Task.CompletedTask;
    }
}