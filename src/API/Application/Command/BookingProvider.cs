using ELibrary_BorrowingService.Application.Command.Exception;
using ELibrary_BorrowingService.Application.Command.Model;
using ELibrary_BorrowingService.Domain.Entity;
using ELibrary_BorrowingService.Domain.Repository;

namespace ELibrary_BorrowingService.Application.Command;

public class BookingProvider : IBookingProvider
{
    private readonly IBookRepository _bookRepository;
    private readonly ICustomerRepository _customerRepository;

    public BookingProvider(IBookRepository bookRepository, ICustomerRepository customerRepository)
    {
        _bookRepository = bookRepository;
        _customerRepository = customerRepository;
    }

    public async Task Book(int bookId, string customerId)
    {
        var book = await GetBookOrThrow(bookId);
        var user = await GetCustomerOrThrow(customerId);

        var booking = new BookingHistory(book, user);
        book.BookBook(booking);

        await _bookRepository.UpdateAsync(book);
        //_bus.Publish()
    }

    public async Task UnBook(int bookId, string customerId)
    {
        var book = await GetBookOrThrow(bookId);
        book.UnBook(customerId);

        await _bookRepository.UpdateAsync(book);
        //_bus.Publish()
    }


    private async Task<Book> GetBookOrThrow(int id)
    {
        var book = await _bookRepository.GetAsync(id);
        if (book is null)
            throw new EntityNotFoundException("Book has not been found");

        return book;
    }

    private async Task<Customer> GetCustomerOrThrow(string id)
    {
        var user = await _customerRepository.GetAsync(id);
        if (user is null)
            throw new EntityNotFoundException("Customer has not been found");

        return user;
    }
}
