using RabbitMqMessages;
using MassTransit;

namespace ELibrary_BorrowingService.Consumers
{
    public class BookAvailabilityChangedConsumer : IConsumer<BookAvailabilityChanged>
    {
        public BookAvailabilityChangedConsumer()
        {
        }

        public async Task Consume(ConsumeContext<BookAvailabilityChanged> context)
        {
            //_logger.LogInformation(context.Message.BookId + " " + context.Message.Amount);
            
            // Change book amount Logic


            Console.WriteLine(context.Message.BookId + " " + context.Message.Amount);
            
        }
    }
}
