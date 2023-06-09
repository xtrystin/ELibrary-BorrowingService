﻿using ELibrary_BorrowingService.Application.Command.Exception;
using ELibrary_BorrowingService.Domain.Entity;
using ELibrary_BorrowingService.Domain.Repository;

namespace ELibrary_BorrowingService.Application.Common;

public class CommonHelpers : ICommonHelpers
{
    private readonly IBookRepository _bookRepository;
    private readonly ICustomerRepository _customerRepository;

    public CommonHelpers(IBookRepository bookRepository, ICustomerRepository customerRepository)
    {
        _bookRepository = bookRepository;
        _customerRepository = customerRepository;
    }
    public async Task<Book> GetBookOrThrow(int id)
    {
        var book = await _bookRepository.GetAsync(id);
        if (book is null)
            throw new EntityNotFoundException("Book has not been found");

        return book;
    }

    public async Task<Customer> GetCustomerOrThrow(string id)
    {
        var user = await _customerRepository.GetAsync(id);
        if (user is null)
            throw new EntityNotFoundException("Customer has not been found");

        return user;
    }
}

public interface ICommonHelpers
{
    Task<Book> GetBookOrThrow(int id);
    Task<Customer> GetCustomerOrThrow(string id);
}
