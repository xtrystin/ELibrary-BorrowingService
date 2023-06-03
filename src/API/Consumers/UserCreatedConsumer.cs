using ELibrary_BorrowingService.Domain.Entity;
using ELibrary_BorrowingService.Domain.Repository;
using MassTransit;
using RabbitMqMessages;

namespace ELibrary_BorrowingService.Consumers;

public class UserCreatedConsumer : IConsumer<UserCreated>
{
    private readonly ICustomerRepository _customerRepository;

    public UserCreatedConsumer(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public async Task Consume(ConsumeContext<UserCreated> context)
    {
        var message = context.Message;
        Customer customer = new(message.UserId, 10, 10);
        await _customerRepository.AddAsync(customer);
    }
}
