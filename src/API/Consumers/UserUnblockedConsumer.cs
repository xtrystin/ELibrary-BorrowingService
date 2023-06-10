using ELibrary_BorrowingService.Domain.Repository;
using MassTransit;
using ServiceBusMessages;

namespace ELibrary_BorrowingService.Consumers;

public class UserUnblockedConsumer : IConsumer<UserUnblocked>
{
    private readonly ICustomerRepository _customerRepository;

    public UserUnblockedConsumer(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public async Task Consume(ConsumeContext<UserUnblocked> context)
    {
        var customer = await _customerRepository.GetAsync(context.Message.UserId);
        if (customer is not null)
        {
            customer.ChangeAccountStatus(isAccountBlocked: false);
            await _customerRepository.UpdateAsync(customer);
        }
    }
}

public class UserUnblockedBrConsumer : IConsumer<UserUnblockedBr>
{
    private readonly ICustomerRepository _customerRepository;

    public UserUnblockedBrConsumer(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public async Task Consume(ConsumeContext<UserUnblockedBr> context)
    {
        var customer = await _customerRepository.GetAsync(context.Message.UserId);
        if (customer is not null)
        {
            customer.ChangeAccountStatus(isAccountBlocked: false);
            await _customerRepository.UpdateAsync(customer);
        }
    }
}