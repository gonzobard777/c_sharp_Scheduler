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
            // Just use the name of your job that you created in the Jobs folder.
            var jobKey = new JobKey("CheckJob");
            q.AddJob<CheckJob>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("CheckJob-trigger")
                //This Cron interval can be described as "run every minute" (when second is zero)
                .WithCronSchedule("0 * * ? * *")
            );
        });
        builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        var app = builder.Build();


        // Configure the HTTP request pipeline.

        app.MapControllers();

        app.Run();
    }
}