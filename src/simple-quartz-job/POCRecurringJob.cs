using Microsoft.Extensions.Logging;
using Quartz;

namespace simple_quartz_job;

public class POCRecurringJob : BaseJob
{
    private readonly ILogger<POCRecurringJob> _logger;
    private readonly IServiceProvider _serviceProvider;

    public POCRecurringJob(ILogger<POCRecurringJob> logger, IServiceProvider serviceProvider) : base(logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteCore(IJobExecutionContext context, DateTime executionTime)
    {
        _logger.LogInformation(
            $"[{DateTime.UtcNow:u}][{await _serviceProvider.GetServiceName()}][{context.Trigger.JobKey}] POC.");

        await Task.CompletedTask;
    }
}