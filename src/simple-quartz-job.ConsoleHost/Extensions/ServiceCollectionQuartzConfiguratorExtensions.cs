using Microsoft.Extensions.Configuration;
using Quartz;
using simple_quartz_job.ConsoleHost.Models;

namespace simple_quartz_job.ConsoleHost.Extensions;

public static class ServiceCollectionQuartzConfiguratorExtensions
{
    public static void AddJobAndTrigger<T>(
        this IServiceCollectionQuartzConfigurator quartz,
        IConfiguration config)
        where T : IJob
    {
        var jobName = typeof(T).Name;
        var configKey = $"Quartz:{jobName}";

        var jobSettings = config.GetSection(configKey).Get<QuartzSetting>();

        if (jobSettings is null)
        {
            throw new Exception($"No Cron schedule found for job in configuration at {configKey}");
        }

        var jobKey = new JobKey(jobName);
        quartz.AddJob<T>(opts =>
            opts.WithIdentity(jobKey)
                .WithDescription(jobSettings.Description)
                .DisallowConcurrentExecution(jobSettings.DisallowConcurrentExecution));

        quartz.AddTrigger(opts => opts
            .ForJob(jobKey)
            .WithIdentity(jobName + "-trigger")
            .WithCronSchedule(jobSettings.Cron));
    }
}