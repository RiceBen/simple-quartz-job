using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Quartz;
using simple_quartz_job;
using simple_quartz_job.ConsoleHost.Extensions;

Debug.WriteLine($"[{DateTime.UtcNow.ToString("u")}] Start {Process.GetCurrentProcess().MainModule?.ModuleName}!");

IConfiguration config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("Config/appsettings.json", false, true)
    .Build();

Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(configHost => { configHost.AddConfiguration(config); })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddQuartz(quartz =>
        {
            quartz.SchedulerId = hostContext.Configuration["Scheduler:Id"] ??
                                 $"{Process.GetCurrentProcess().MainModule?.ModuleName}";
            quartz.UseMicrosoftDependencyInjectionJobFactory();
            quartz.UseSimpleTypeLoader();
            quartz.UseInMemoryStore();
            quartz.UseDefaultThreadPool(tp =>
            {
                tp.MaxConcurrency = int.Parse(hostContext.Configuration["Scheduler:MaxConcurrency"] ?? "10");
            });

            quartz.AddJobAndTrigger<HealthyCheckJob>(hostContext.Configuration);
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
            options.AwaitApplicationStarted = true;
        });
    }).Build().Run();