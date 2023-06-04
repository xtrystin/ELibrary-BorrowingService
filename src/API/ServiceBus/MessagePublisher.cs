using MassTransit;
using ServiceBusMessages;

namespace ELibrary_BorrowingService.ServiceBus;

public class MessagePublisher : IMessagePublisher
{
    private readonly IBus _bus;
    private readonly IConfiguration _configuration;

    public MessagePublisher(IBus bus, IConfiguration configuration)
    {
        _bus = bus;
        _configuration = configuration;
    }

    public async Task Publish<T>(T message)
    {
        if (_configuration["Flags:UserRabbitMq"] == "1")
        {
            await _bus.Publish(message);
        }
        else
        {
            // Publisg to many queues -> because Basic Tier ASB allowed only 1-1 queues, no topics
            if (message is BookAvailabilityChanged)
            {
                var m = message as BookAvailabilityChanged;
                var bookServiceMessage = new BookAvailabilityChangedBk() { BookId = m.BookId, Amount = m.Amount };

                await _bus.Send(bookServiceMessage);
            }
            else if (message is OvertimeReturn)
            {
                var m = message as OvertimeReturn;
                var userServiceMessage = new OvertimeReturnU() { UserId = m.UserId, AmountToPay = m.AmountToPay };

                await _bus.Send(userServiceMessage);
            }
            else
            {
                // send to one queue
                await _bus.Send(message);
            }
        }
    }
}