using ELibrary_BorrowingService.Application.Command;
using ELibrary_BorrowingService.Application.Common;
using ELibrary_BorrowingService.Application.Query;

namespace ELibrary_BorrowingService.Application;

public static class ProviderCollectionExtension
{
    public static IServiceCollection AddProviderCollection(this IServiceCollection services)
    {
        // Read Providers
        services.AddScoped<IBorrowReadProvider, BorrowReadProvider>();
        services.AddScoped<IBookingReadProvider, BookingReadProvider>();

        // Write Providers
        services.AddScoped<IBookingProvider, BookingProvider>();
        services.AddScoped<IBorrowProvider, BorrowProvider>();

        //Common
        services.AddScoped<ICommonHelpers, CommonHelpers>();
        return services;
    }
}
