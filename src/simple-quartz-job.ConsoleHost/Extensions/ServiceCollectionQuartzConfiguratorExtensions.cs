﻿using Microsoft.Extensions.Configuration;
using Quartz;

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

        if (jobSettings is null) throw new Exception($"No Cron schedule found for job in configuration at {configKey}");

        if (jobSettings.Enable == false) return;

        var jobKey = new JobKey(jobName);
        quartz.AddJob<T>(opts =>
            opts.WithIdentity(jobKey)
                .WithDescription(jobSettings.Description)
                .DisallowConcurrentExecution(jobSettings.DisallowConcurrentExecution));

        quartz.AddTrigger(opts => opts
            .ForJob(jobKey)
            .WithIdentity(jobName + "-trigger")
            .WithPriority(jobSettings.Priority)
            .WithCronSchedule(jobSettings.Cron, builder => builder.WithMisfireHandlingInstructionDoNothing()));
    }

    public static void AddJobsAndTriggerAll(
        this IServiceCollectionQuartzConfigurator quartz,
        IConfiguration config)
    {
        var serviceAssembly = typeof(BaseJob).Assembly;

        var jobTypes = serviceAssembly.GetTypes()
            .Where(allTypes =>
                typeof(BaseJob).IsAssignableFrom(allTypes)
                && allTypes.IsAbstract is false);

        var baseMethod = typeof(ServiceCollectionQuartzConfiguratorExtensions).GetMethod(
            nameof(AddJobAndTrigger));

        if (baseMethod is null)
            throw new ApplicationException(
                $"Cannot find method {nameof(AddJobAndTrigger)} in {nameof(ServiceCollectionQuartzConfiguratorExtensions)} class");

        foreach (var job in jobTypes)
        {
            var genericMethod = baseMethod.MakeGenericMethod(job);
            genericMethod.Invoke(null, new object?[] { quartz, config });
        }
    }
}