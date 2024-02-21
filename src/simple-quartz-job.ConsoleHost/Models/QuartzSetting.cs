namespace simple_quartz_job.ConsoleHost.Models;

public class QuartzSetting
{
    public string Cron { get; set; }

    public string Description { get; set; }

    public bool DisallowConcurrentExecution { get; set; }
    
    /// <summary>
    /// Job priority. When more than one Trigger have the same fire time,
    /// the scheduler will fire the one with the highest priority first.
    /// </summary>
    /// <remarks>
    /// Bigger number higher priority, default is 5.
    /// "https://www.quartz-scheduler.net/documentation/quartz-2.x/tutorial/more-about-triggers.html#priority"
    /// </remarks>
    public int Priority { get; set; }
}