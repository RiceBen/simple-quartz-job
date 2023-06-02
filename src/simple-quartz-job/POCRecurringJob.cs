using Microsoft.Extensions.Logging;
using Quartz;

namespace simple_quartz_job;

public class POCRecurringJob : IJob
{
    private readonly ILogger<POCRecurringJob> _logger;
    private readonly IServiceProvider _serviceProvider;

    public POCRecurringJob(ILogger<POCRecurringJob> logger, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        if (context.CancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation($"[{DateTime.UtcNow.ToString("u")}] {context.JobDetail.JobType.Name} is going to stop.");
            return;
        }
        
        _logger.LogInformation(
            $"[{DateTime.UtcNow.ToString("u")}][{await _serviceProvider.GetServiceName()}][{context.Trigger.JobKey}] POC.");

        await Task.CompletedTask;
    }
}