using ELibrary_BorrowingService.Application.Common;
using ELibrary_BorrowingService.Domain.Entity;
using ELibrary_BorrowingService.Domain.Exception;
using ELibrary_BorrowingService.Domain.Repository;
using ELibrary_BorrowingService.Infrastructure.EF.Repository;
using ELibrary_BorrowingService.ServiceBus;
using ServiceBusMessages;

namespace ELibrary_BorrowingService.Application.Command;

public class BorrowProvider : IBorrowProvider
{
    private readonly IBookRepository _bookRepository;
    private readonly ICommonHelpers _commonHelpers;
    private readonly IMessagePublisher _messagePublisher;
    private readonly ICustomerRepository _customerRepository;

    public BorrowProvider(IBookRepository bookRepository, ICommonHelpers commonHelpers,
        IMessagePublisher messagePublisher, ICustomerRepository customerRepository)
    {
        _bookRepository = bookRepository;
        _commonHelpers = commonHelpers;
        _messagePublisher = messagePublisher;
        _customerRepository = customerRepository;
    }

    public async Task Borrow(int bookId, string customerId)
    {
        var book = await _commonHelpers.GetBookOrThrow(bookId);
        var user = await _commonHelpers.GetCustomerOrThrow(customerId);

        var borrowing = new BorrowingHistory(book, user);
        try
        {
            book.Borrow(borrowing);
        }
        catch (BorrowAfterBookingException)
        {
            await _bookRepository.UpdateAsync(book);
            return;
        }

        await _bookRepository.UpdateAsync(book);
        await _customerRepository.UpdateAsync(user);

        var availabilityChanged = new BookAvailabilityChanged() { BookId = bookId, Amount = -1 };
        await _messagePublisher.Publish(availabilityChanged);
    }

    public async Task Return(int bookId, string customerId)
    {
        var book = await _commonHelpers.GetBookOrThrow(bookId);
        try
        {
            book.Return(customerId);
        }
        catch (OverTimeReturnException ex)
        {
            if (Decimal.TryParse(ex.Message, out decimal penalty))
            {
                var overtimeReturn = new OvertimeReturn() { UserId = customerId, AmountToPay = penalty };
                await _messagePublisher.Publish(overtimeReturn);
            }
        }

        await _bookRepository.UpdateAsync(book);
        await _customerRepository.UpdateAsync(await _customerRepository.GetAsync(customerId));

        var availabilityChanged = new BookAvailabilityChanged() { BookId = bookId, Amount = 1 };
        await _messagePublisher.Publish(availabilityChanged);

    }
}
