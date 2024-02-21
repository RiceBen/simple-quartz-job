﻿using Microsoft.Extensions.Logging;
using Quartz;

namespace simple_quartz_job;

public class HealthyCheckJob : IJob
{
    private readonly ILogger<HealthyCheckJob> _logger;
    private readonly IServiceProvider _serviceProvider;

    public HealthyCheckJob(ILogger<HealthyCheckJob> logger, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        // need to check IJobExecutionContext'sCancellationToken.IsCancellationRequested if job canceled then stop it.
        if (context.CancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation($"[{DateTime.UtcNow.ToString("u")}] {context.JobDetail.JobType.Name} is going to stop.");
            return;
        }
        
        _logger.LogInformation(
            $"[{DateTime.UtcNow.ToString("u")}][{await _serviceProvider.GetServiceName()}][{context.Trigger.JobKey}] Healthy.");
        
        await Task.CompletedTask;
    }
}