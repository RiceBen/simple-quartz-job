namespace simple_quartz_job;

public class DemoServiceProvider : IServiceProvider
{
    public async Task<string> GetServiceName()
    {
        return await Task.FromResult(Guid.NewGuid().ToString());
    }
}