using Quartz;
using Quartz.Impl;

namespace WebApi.Jobs;

public class CheckJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        var detail = (JobDetailImpl)context.JobDetail;
        
        Console.WriteLine(@$"Job ""{detail.Name}"": {DateTime.Now}");
        
        return Task.CompletedTask;
    }
}