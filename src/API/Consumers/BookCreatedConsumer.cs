using ELibrary_BorrowingService.Domain.Entity;
using ELibrary_BorrowingService.Domain.Repository;
using MassTransit;
using ServiceBusMessages;

namespace ELibrary_BorrowingService.Consumers;

public class BookCreatedConsumer : IConsumer<BookCreated>
{
    private readonly IBookRepository _bookRepository;

    public BookCreatedConsumer(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    public async Task Consume(ConsumeContext<BookCreated> context)
    {
        var message = context.Message;
        var book = await _bookRepository.GetAsync(message.BookId);
        if (book is null)
        {
            book = new Book(message.BookId, message.Amount);
            await _bookRepository.AddAsync(book);
        }
    }
}

public class BookCreatedBrConsumer : IConsumer<BookCreatedBr>
{
    private readonly IBookRepository _bookRepository;

    public BookCreatedBrConsumer(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    public async Task Consume(ConsumeContext<BookCreatedBr> context)
    {
        var message = context.Message;
        var book = await _bookRepository.GetAsync(message.BookId);
        if (book is null)
        {
            book = new Book(message.BookId, message.Amount);
            await _bookRepository.AddAsync(book);
        }
    }
}
