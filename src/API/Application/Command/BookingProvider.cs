using ELibrary_BorrowingService.Application.Common;
using ELibrary_BorrowingService.Domain.Entity;
using ELibrary_BorrowingService.Domain.Repository;
using ELibrary_BorrowingService.ServiceBus;
using ServiceBusMessages;

namespace ELibrary_BorrowingService.Application.Command;

public class BookingProvider : IBookingProvider
{
    private readonly IBookRepository _bookRepository;
    private readonly ICommonHelpers _commonHelpers;
    private readonly IMessagePublisher _messagePublisher;

    public BookingProvider(IBookRepository bookRepository, ICommonHelpers commonHelpers,
        IMessagePublisher messagePublisher)
    {
        _bookRepository = bookRepository;
        _commonHelpers = commonHelpers;
        _messagePublisher = messagePublisher;
    }

    public async Task Book(int bookId, string customerId)
    {
        var book = await _commonHelpers.GetBookOrThrow(bookId);
        var user = await _commonHelpers.GetCustomerOrThrow(customerId);

        var booking = new BookingHistory(book, user);
        book.BookBook(booking);

        await _bookRepository.UpdateAsync(book);
        var availabilityChanged = new BookAvailabilityChanged() { BookId = bookId, Amount = -1 };
        await _messagePublisher.Publish(availabilityChanged);
    }

    public async Task UnBook(int bookId, string customerId)
    {
        var book = await _commonHelpers.GetBookOrThrow(bookId);
        book.UnBook(customerId);

        await _bookRepository.UpdateAsync(book);
        var availabilityChanged = new BookAvailabilityChanged() { BookId = bookId, Amount = 1 };
        await _messagePublisher.Publish(availabilityChanged);
    }
}
