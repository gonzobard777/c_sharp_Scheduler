using Quartz;

namespace WebApi.Jobs;

public class CheckJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine(@$"Job executed: {DateTime.Now}");
        
        return Task.CompletedTask;
    }
}