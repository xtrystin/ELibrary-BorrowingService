using ELibrary_BorrowingService.Domain.Repository;
using ELibrary_BorrowingService.Infrastructure.EF.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ELibrary_BorrowingService.Infrastructure.EF
{
    public static class Extensions
    {
        public static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BookDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgresResourceDb")));

            // register repositories
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IBookingHistoryRepository, BookingHistoryRepository>();

            return services;
        }
    }
}
