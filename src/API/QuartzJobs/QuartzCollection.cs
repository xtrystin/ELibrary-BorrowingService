using ELibrary_BorrowingService.QuartzJobs;
using Quartz;

namespace ELibrary_BorrowingService.Jobs;

public static class QuartzCollection
{
    public static IServiceCollection AddQuartzJobs(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();

            var jobKey = new JobKey(nameof(DeactivateBookingsJob));
            q.AddJob<DeactivateBookingsJob>(opts => opts.WithIdentity(jobKey));

            var deactivateBookingsCron = configuration["JobsCron:DeactivateBookingsJob"];
            q.AddTrigger(opts => opts.ForJob(jobKey).WithIdentity(jobKey + "-trigger").WithCronSchedule(deactivateBookingsCron));
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        return services;
    }
}
