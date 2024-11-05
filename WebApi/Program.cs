using Quartz;
using WebApi.Jobs;

namespace WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        // Add services to the container.

        builder.Services.AddControllers();

        builder.Services.AddQuartz(q =>
        {
            var addJob = (string name, string cronExpression) =>
            {
                var jobKey = new JobKey(name);
                q.AddJob<CheckJob>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithCronSchedule(cronExpression)
                );
            };

            addJob("EveryMinuteJob", "0 * * ? * *");
            addJob("Every30SecondJob", "0/30 * * ? * *");
            
        });
        builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        var app = builder.Build();


        // Configure the HTTP request pipeline.

        app.MapControllers();

        app.Run();
    }
}