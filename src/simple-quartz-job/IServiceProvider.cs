namespace simple_quartz_job;

public interface IServiceProvider
{
    Task<string> GetServiceName();
}