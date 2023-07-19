using Microsoft.Extensions.Logging;
using Quartz;

namespace simple_quartz_job;

public class HealthyCheckJob : BaseJob
{
    private readonly ILogger<HealthyCheckJob> _logger;
    private readonly IServiceProvider _serviceProvider;

    public HealthyCheckJob(ILogger<HealthyCheckJob> logger, IServiceProvider serviceProvider) : base(logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteCore(IJobExecutionContext context)
    {
        _logger.LogInformation(
            $"[{DateTime.UtcNow.ToString("u")}][{await _serviceProvider.GetServiceName()}][{context.Trigger.JobKey}] Healthy.");

        await Task.CompletedTask;
    }
}