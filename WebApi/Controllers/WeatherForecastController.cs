using Microsoft.AspNetCore.Mvc;
using Quartz;
using Quartz.Impl.Matchers;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ISchedulerFactory schedulerFactory;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, ISchedulerFactory schedulerFactory)
    {
        _logger = logger;
        this.schedulerFactory = schedulerFactory ?? throw new ArgumentNullException(nameof(schedulerFactory));
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }

    [HttpGet]
    [Route("health")]
    public async Task<IActionResult> Check()
    {
        var scheduler = await schedulerFactory.GetScheduler().ConfigureAwait(false);
        if (!scheduler.IsStarted)
        {
            return Ok("Not stated");
        }

        // https://github.com/quartznet/quartznet/blob/main/src/Quartz.AspNetCore/AspNetCore/HealthChecks/QuartzHealthCheck.cs
        // try
        // {
        //     // Ask for a job we know doesn't exist
        //     await scheduler.CheckExists(new JobKey(Guid.NewGuid().ToString()), cancellationToken).ConfigureAwait(false);
        // }
        // catch (SchedulerException)
        // {
        //     return HealthCheckResult.Unhealthy("Quartz scheduler cannot connect to the store");
        // }


        // Getting all the active jobs from Quartz.NET scheduler
        // https://stackoverflow.com/questions/6648815/getting-all-the-active-jobs-from-quartz-net-scheduler


        // https://stackoverflow.com/questions/12489450/get-all-jobs-in-quartz-net-2-0#answer-78394274
        try
        {
            var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());

            foreach (var jobKey in jobKeys)
            {
                var jobDetail = await scheduler.GetJobDetail(jobKey);

                if (jobDetail != null)
                {
                    var triggers = await scheduler.GetTriggersOfJob(jobKey);
                    foreach (var trigger in triggers)
                    {
                        var triggerState = scheduler.GetTriggerState(trigger.Key).Result;
                        var state = "";
                        if (TriggerState.Normal.Equals(triggerState))
                            state = "Normal";
                        else if (TriggerState.Paused.Equals(triggerState))
                            state = "Paused";
                        else if (TriggerState.Complete.Equals(triggerState))
                            state = "Complete";
                        else if (TriggerState.Error.Equals(triggerState))
                            state = "Error";
                        else if (TriggerState.Blocked.Equals(triggerState))
                            state = "Blocked";
                        else if (TriggerState.None.Equals(triggerState))
                            state = "None";
                        if (state == "")
                            state = triggerState.ToString();
                        Console.WriteLine($"State: {state}");
                        Console.WriteLine("Job Key = " + jobKey.Name);
                        Console.WriteLine("Job Schduled Time = " + trigger.GetNextFireTimeUtc()?.ToString() ?? "Not scheduled");
                        Console.WriteLine("Job Group = " + jobKey.Group);
                        Console.WriteLine("Job Trigger Name= " + trigger.Key.Name);
                    }
                }
            }
        }
        catch (SchedulerException)
        {
            return Ok("Quartz scheduler cannot connect to the store");
        }

        return Ok("Quartz scheduler is ready");
    }
}