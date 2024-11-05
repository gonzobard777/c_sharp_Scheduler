using Quartz;
using WebApi.Helpers;
using WebApi.Jobs;

namespace WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;


        // Add services to the container.
        builder.Services.AddControllers();

        // Расписание
        builder.Services.AddQuartz(q =>
        {
            var triggers = configuration
                .GetSection(TriggerEndpointConfig.group)
                .Get<List<TriggerEndpointData>>();
            
            foreach (var item in triggers)
            {
                var dataMap = new JobDataMap { { TriggerEndpointConfig.dataMapKey, item } };
                
                var jobKey = new JobKey(item.Endpoint);
                q.AddJob<TriggerEndpoint>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .UsingJobData(dataMap)
                    .WithCronSchedule(item.Schedule)
                );
            }
        });
        builder.Services.AddQuartzHostedService(q =>
        {
            q.WaitForJobsToComplete = true; // when shutting down we want jobs to complete gracefully
        });


        // Configure the HTTP request pipeline.
        var app = builder.Build();

        app.MapControllers();

        app.Run();
    }
}