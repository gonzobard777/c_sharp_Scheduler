# Scheduler

Надо по расписанию дергать какую-нибудь апишку.  
Список апишек и расписание для дергания определен в [appsettings.json](https://github.com/gonzobard777/c_sharp_Scheduler/blob/QuartzAspNetCore/WebApi/appsettings.json#L2).

- [Конструктор расписания](https://www.freeformatter.com/cron-expression-generator-quartz.html)
- [ASP.NET Core Integration](https://www.quartz-scheduler.net/documentation/quartz-3.x/packages/aspnet-core-integration.html)

Добавил что-то вроде Health Check.
Смотри метод [/weatherforecast/health](https://github.com/gonzobard777/c_sharp_Scheduler/blob/QuartzAspNetCore/WebApi/Controllers/WeatherForecastController.cs#L39)

# Ссылки
- [How to determine if Job is running for the first time?](https://github.com/quartznet/quartznet/discussions/2686)
- [Rescheduling Jobs](https://www.quartz-scheduler.net/documentation/quartz-3.x/how-tos/rescheduling-jobs.html#rescheduling-jobs)
- [Handling Job Failures in Quartz with Retries](https://hackernoon.com/handling-job-failures-in-quartz-with-retries)