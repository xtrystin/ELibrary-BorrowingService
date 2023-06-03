using ELibrary_BorrowingService.Application.Command;

namespace ELibrary_BorrowingService.Application;

public static class ProviderCollectionExtension
{
    public static IServiceCollection AddProviderCollection(this IServiceCollection services)
    {
        // Read Providers


        // Write Providers
        services.AddScoped<IBookingProvider, BookingProvider>();
        services.AddScoped<IBorrowProvider, BorrowProvider>();

        return services;
    }
}
