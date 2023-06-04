using ELibrary_BorrowingService.Domain.Entity;
using ELibrary_BorrowingService.Domain.Repository;
using MassTransit;
using ServiceBusMessages;

namespace ELibrary_BorrowingService.Consumers;

public class UserCreatedConsumer : IConsumer<UserCreated>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly int MAX_BOOK_TO_BORROW = 10;
    private readonly int MAX_BOOK_TO_BOOK = 10;

    public UserCreatedConsumer(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public async Task Consume(ConsumeContext<UserCreated> context)
    {
        var message = context.Message;
        var customer = await _customerRepository.GetAsync(message.UserId);
        if (customer is null)
        {
            customer = new Customer(message.UserId, MAX_BOOK_TO_BOOK, MAX_BOOK_TO_BORROW);
            await _customerRepository.AddAsync(customer);
        }
    }
}

public class UserCreatedBrConsumer : IConsumer<UserCreatedBr>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly int MAX_BOOK_TO_BORROW = 10;
    private readonly int MAX_BOOK_TO_BOOK = 10;

    public UserCreatedBrConsumer(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public async Task Consume(ConsumeContext<UserCreatedBr> context)
    {
        var message = context.Message;
        var customer = await _customerRepository.GetAsync(message.UserId);
        if (customer is null)
        {
            customer = new Customer(message.UserId, MAX_BOOK_TO_BOOK, MAX_BOOK_TO_BORROW);
            await _customerRepository.AddAsync(customer);
        }
    }
}
