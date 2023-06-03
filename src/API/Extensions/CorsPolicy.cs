namespace ELibrary_BorrowingService.Extensions;

public static class CorsPolicy
{
    public static IServiceCollection AddOpenCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(policy =>
        {
            policy.AddPolicy("OpenCorsPolicy", opt =>
                opt.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
        });

        return services;
    }
}
