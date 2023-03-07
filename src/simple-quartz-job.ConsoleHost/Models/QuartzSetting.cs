namespace simple_quartz_job.ConsoleHost.Models;

public class QuartzSetting
{
    public string Cron { get; set; }

    public string Description { get; set; }

    public bool DisallowConcurrentExecution { get; set; }
}