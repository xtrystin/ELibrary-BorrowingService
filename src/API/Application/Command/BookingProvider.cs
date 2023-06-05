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
    private readonly ICustomerRepository _customerRepository;

    public BookingProvider(IBookRepository bookRepository, ICommonHelpers commonHelpers,
        IMessagePublisher messagePublisher, ICustomerRepository customerRepository)
    {
        _bookRepository = bookRepository;
        _commonHelpers = commonHelpers;
        _messagePublisher = messagePublisher;
        _customerRepository = customerRepository;
    }

    public async Task Book(int bookId, string customerId)
    {
        var book = await _commonHelpers.GetBookOrThrow(bookId);
        var user = await _commonHelpers.GetCustomerOrThrow(customerId);

        var booking = new BookingHistory(book, user);
        book.BookBook(booking);

        await _bookRepository.UpdateAsync(book);
        await _customerRepository.UpdateAsync(user);

        var availabilityChanged = new BookAvailabilityChanged() { BookId = bookId, Amount = -1 };
        await _messagePublisher.Publish(availabilityChanged);
    }

    public async Task UnBook(int bookId, string customerId)
    {
        var book = await _commonHelpers.GetBookOrThrow(bookId);
        book.UnBook(customerId);

        await _bookRepository.UpdateAsync(book);
        await _customerRepository.UpdateAsync(await _customerRepository.GetAsync(customerId));

        var availabilityChanged = new BookAvailabilityChanged() { BookId = bookId, Amount = 1 };
        await _messagePublisher.Publish(availabilityChanged);
    }
}
