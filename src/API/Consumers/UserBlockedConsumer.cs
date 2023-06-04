using ELibrary_BorrowingService.Domain.Repository;
using MassTransit;
using ServiceBusMessages;

namespace ELibrary_BorrowingService.Consumers;

public class UserBlockedConsumer : IConsumer<UserBlocked>
{
    private readonly ICustomerRepository _customerRepository;

    public UserBlockedConsumer(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public async Task Consume(ConsumeContext<UserBlocked> context)
    {
        var customer = await _customerRepository.GetAsync(context.Message.UserId);
        if (customer is not null)
        {
            customer.ChangeAccountStatus(isAccountBlocked: true);
            await _customerRepository.UpdateAsync(customer);
        }
    }
}

public class UserBlockedBrConsumer : IConsumer<UserBlockedBr>
{
    private readonly ICustomerRepository _customerRepository;

    public UserBlockedBrConsumer(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public async Task Consume(ConsumeContext<UserBlockedBr> context)
    {
        var customer = await _customerRepository.GetAsync(context.Message.UserId);
        if (customer is not null)
        {
            customer.ChangeAccountStatus(isAccountBlocked: true);
            await _customerRepository.UpdateAsync(customer);
        }
    }
}
