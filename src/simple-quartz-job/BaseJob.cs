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

    protected abstract Task ExecuteCore(IJobExecutionContext context);

    public async Task Execute(IJobExecutionContext context)
    {
        if (context.CancellationToken.IsCancellationRequested)
        {
            Logger.LogInformation(
                $"[{this.GetType().Name}] is going to stop.");
            return;
        }

        var currentTime = DateTime.UtcNow;
        
        try
        {
            Logger.LogInformation($"[{this.GetType().Name}][{currentTime.Hour}] StartTime:{currentTime:u}");
            await ExecuteCore(context);
            Logger.LogInformation($"[{this.GetType().Name}][{currentTime.Hour}] EndTime: {DateTime.UtcNow:u}, Time duration: {(DateTime.UtcNow-currentTime).TotalMilliseconds}");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"[{this.GetType().Name}] Exception occurred while executing job.");
        }
    }
}