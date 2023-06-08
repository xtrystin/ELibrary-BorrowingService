using ELibrary_BorrowingService.Domain.Repository;
using Quartz;

namespace ELibrary_BorrowingService.QuartzJobs;

public class DeactivateBookingsJob : IJob
{
    private readonly IBookingHistoryRepository _bookingHistoryRepository;

    public DeactivateBookingsJob(IBookingHistoryRepository bookingHistoryRepository)
    {
        _bookingHistoryRepository = bookingHistoryRepository;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        var bookings = await _bookingHistoryRepository.GetWithExceededDate(DateTime.UtcNow);
        foreach (var booking in bookings)
            booking.Unbook(unBookAfterBorrow: false);

        await _bookingHistoryRepository.UpdateRangeAsync(bookings);
    }
}
