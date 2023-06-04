namespace ELibrary_BorrowingService.ServiceBus;

public interface IMessagePublisher
{
    Task Publish<T>(T message);
}
