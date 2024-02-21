using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using simple_quartz_job;
using simple_quartz_job.ConsoleHost.Extensions;
using IServiceProvider = simple_quartz_job.IServiceProvider;

public class Program
{
    public static async Task Main(string[] args)
    {
        Debug.WriteLine(
            $"[{DateTime.UtcNow:u}] Start {Process.GetCurrentProcess().MainModule?.ModuleName}!");

        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("Config/appsettings.json", false, true)
            .Build();

        await Host.CreateDefaultBuilder(args)
            .ConfigureHostConfiguration(configHost => { configHost.AddConfiguration(config); })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddLogging();
                services.AddSingleton<IServiceProvider, DemoServiceProvider>();
                services.AddQuartz(quartz =>
                {
                    quartz.SchedulerId = string.IsNullOrEmpty(hostContext.Configuration["Quartz:Scheduler:Id"])
                        ? $"{hostContext.Configuration["Quartz:Scheduler:Id"]}-{Environment.MachineName}"
                        : $"{Process.GetCurrentProcess().MainModule?.ModuleName}";
                    quartz.InterruptJobsOnShutdownWithWait = true;
                    quartz.MisfireThreshold = new TimeSpan(60000);
                    quartz.MaxBatchSize = 5;
                    quartz.UseTimeZoneConverter();
                    quartz.UseSimpleTypeLoader();
                    quartz.UseInMemoryStore();
                    quartz.UseDefaultThreadPool(tp =>
                    {
                        tp.MaxConcurrency =
                            int.Parse(hostContext.Configuration["Quartz:Scheduler:MaxConcurrency"] ?? "5");
                    });

                    quartz.AddJobsAndTriggerAll(hostContext.Configuration);
                });

                services.AddQuartzHostedService(options =>
                {
                    options.WaitForJobsToComplete = true;
                    options.AwaitApplicationStarted = true;
                });
            }).Build().RunAsync();
    }
}