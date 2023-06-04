using ELibrary_BorrowingService.Application.Common;
using ELibrary_BorrowingService.Domain.Repository;
using MassTransit;
using ServiceBusMessages;

namespace ELibrary_BorrowingService.Consumers
{
    public class BookAvailabilityChangedConsumer : IConsumer<BookAvailabilityChanged>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICommonHelpers _commonHelpers;

        public BookAvailabilityChangedConsumer(IBookRepository bookRepository, ICommonHelpers commonHelpers)
        {
            _bookRepository = bookRepository;
            _commonHelpers = commonHelpers;
        }

        public async Task Consume(ConsumeContext<BookAvailabilityChanged> context)
        {
            var message = context.Message;
            var book = await _commonHelpers.GetBookOrThrow(message.BookId);
            
            book.ChangeBookAvailability(message.Amount);
            await _bookRepository.UpdateAsync(book);
        }
    }

    public class BookAvailabilityChangedBrConsumer : IConsumer<BookAvailabilityChangedBr>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICommonHelpers _commonHelpers;

        public BookAvailabilityChangedBrConsumer(IBookRepository bookRepository, ICommonHelpers commonHelpers)
        {
            _bookRepository = bookRepository;
            _commonHelpers = commonHelpers;
        }

        public async Task Consume(ConsumeContext<BookAvailabilityChangedBr> context)
        {
            var message = context.Message;
            var book = await _commonHelpers.GetBookOrThrow(message.BookId);

            book.ChangeBookAvailability(message.Amount);
            await _bookRepository.UpdateAsync(book);
        }
    }
}
