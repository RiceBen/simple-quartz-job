using Microsoft.Extensions.Logging;
using Quartz;

namespace simple_quartz_job;

public abstract class BaseJob : IJob
{
    protected readonly ILogger Logger;

    public BaseJob(ILogger logger)
    {
        Logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        if (context.CancellationToken.IsCancellationRequested)
        {
            Logger.LogInformation(
                $"[{GetType().Name}] is going to stop.");
            return;
        }

        var currentTime = DateTime.UtcNow;

        try
        {
            Logger.LogInformation($"[{GetType().Name}][{currentTime.Hour}] StartTime:{currentTime:u}");
            await ExecuteCore(context, currentTime);
            Logger.LogInformation(
                $"[{GetType().Name}][{currentTime.Hour}] EndTime: {DateTime.UtcNow:u}, Time duration: {(DateTime.UtcNow - currentTime).TotalSeconds} sec.");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"[{GetType().Name}] Exception occurred while executing job.");
        }
    }

    protected abstract Task ExecuteCore(IJobExecutionContext context, DateTime executionTime);
}