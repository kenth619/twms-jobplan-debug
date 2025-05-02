using Microsoft.EntityFrameworkCore;
using Quartz;
using TWMSServer.Model;

namespace TWMSServer.Jobs
{
    public class JobScheduleInitializer(ILogger<JobScheduleInitializer> logger, ISchedulerFactory schedulerFactory, IDbContextFactory<TWMSServerContext> contextFactory) : IHostedService
    {
        private readonly ILogger<JobScheduleInitializer> _logger = logger;
        private readonly ISchedulerFactory _schedulerFactory = schedulerFactory;
        private readonly IDbContextFactory<TWMSServerContext> _contextFactory = contextFactory;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting JobScheduleInitializer...");
            
            try
            {
                var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
                using var context = _contextFactory.CreateDbContext();
                
                // if (!await context.JobSchedules.AnyAsync(js => js.JobName == "NewListJob", cancellationToken))
                // {
                //     var defaultSchedule = new JobSchedule
                //     {
                //         JobName = "NewListJob",
                //         CronExpression = "0 0 1 1 * ?", // At 1:00 AM on the 1st day of every month
                //         IsEnabled = true,
                //         CreatedAt = DateTime.Now
                //     };
                    
                //     context.JobSchedules.Add(defaultSchedule);
                //     await context.SaveChangesAsync(cancellationToken);
                    
                //     _logger.LogInformation("Created default schedule for NewListJob: {CronExpression}", defaultSchedule.CronExpression);
                // }
                
                var jobSchedules = await context.JobSchedules.Where(js => js.IsEnabled).ToListAsync(cancellationToken);
                
                foreach (var jobSchedule in jobSchedules)
                {
                    await ScheduleJob(scheduler, jobSchedule, cancellationToken);
                }
                
                _logger.LogInformation("JobScheduleInitializer completed. Scheduled {Count} jobs.", jobSchedules.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing job schedules");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        
        private async Task ScheduleJob(IScheduler scheduler, JobSchedule jobSchedule, CancellationToken cancellationToken)
        {
            try
            {
                // Check if the job exists
                var jobKey = new JobKey(jobSchedule.JobName);
                if (!await scheduler.CheckExists(jobKey, cancellationToken))
                {
                    _logger.LogWarning("Job {JobName} does not exist in the scheduler. It should be registered in Program.cs.", jobSchedule.JobName);
                    return;
                }
                
                // Create a trigger with the cron expression
                var trigger = TriggerBuilder.Create()
                    .WithIdentity($"{jobSchedule.JobName}Trigger")
                    .WithCronSchedule(jobSchedule.CronExpression)
                    .ForJob(jobKey)
                    .Build();
                
                // Delete any existing triggers for this job
                var existingTriggers = await scheduler.GetTriggersOfJob(jobKey, cancellationToken);
                foreach (var existingTrigger in existingTriggers)
                {
                    await scheduler.UnscheduleJob(existingTrigger.Key, cancellationToken);
                }
                
                // Schedule the job with the new trigger
                await scheduler.ScheduleJob(trigger, cancellationToken);
                
                _logger.LogInformation("Scheduled job {JobName} with cron expression: {CronExpression}", 
                    jobSchedule.JobName, jobSchedule.CronExpression);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error scheduling job {JobName}", jobSchedule.JobName);
            }
        }
    }
}