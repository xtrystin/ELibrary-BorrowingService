using ELibrary_BorrowingService.Domain.Repository;
using MassTransit;
using ServiceBusMessages;

namespace ELibrary_BorrowingService.Consumers;

public class UserDeletedConsumer : IConsumer<UserDeleted>
{
    private readonly ICustomerRepository _customerRepository;

    public UserDeletedConsumer(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public async Task Consume(ConsumeContext<UserDeleted> context)
    {
        var customer = await _customerRepository.GetAsync(context.Message.UserId);
        if (customer is not null) 
        {
            customer.DeleteCustomer();
            await _customerRepository.UpdateAsync(customer);
        }
    }
}

public class UserDeletedBrConsumer : IConsumer<UserDeletedBr>
{
    private readonly ICustomerRepository _customerRepository;

    public UserDeletedBrConsumer(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public async Task Consume(ConsumeContext<UserDeletedBr> context)
    {
        var customer = await _customerRepository.GetAsync(context.Message.UserId);
        if (customer is not null) 
        {
            customer.DeleteCustomer();
            await _customerRepository.UpdateAsync(customer);
        }
    }
}
