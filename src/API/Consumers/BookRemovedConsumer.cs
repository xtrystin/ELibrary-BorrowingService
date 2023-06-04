using ELibrary_BorrowingService.Domain.Repository;
using MassTransit;
using ServiceBusMessages;

namespace ELibrary_BorrowingService.Consumers;

public class BookRemovedConsumer : IConsumer<BookRemoved>
{
    private readonly IBookRepository _bookRepository;

    public BookRemovedConsumer(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    public async Task Consume(ConsumeContext<BookRemoved> context)
    {
        var book = await _bookRepository.GetAsync(context.Message.BookId);
        if (book is not null)
        {
            book.RemoveBook();
            await _bookRepository.UpdateAsync(book);
        }
    }
}

public class BookRemovedBrConsumer : IConsumer<BookRemovedBr>
{
    private readonly IBookRepository _bookRepository;

    public BookRemovedBrConsumer(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    public async Task Consume(ConsumeContext<BookRemovedBr> context)
    {
        var book = await _bookRepository.GetAsync(context.Message.BookId);
        if (book is not null)
        {
            book.RemoveBook();
            await _bookRepository.UpdateAsync(book);
        }
    }
}
