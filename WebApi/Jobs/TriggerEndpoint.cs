using Quartz;
using WebApi.Helpers;

namespace WebApi.Jobs;

public class TriggerEndpoint : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.MergedJobDataMap;
        var data = (TriggerEndpointData)dataMap.Get(TriggerEndpointConfig.dataMapKey);

        Console.WriteLine(@$"Job ""{data.Endpoint}"": {DateTime.Now}");

        return Task.CompletedTask;
    }
}