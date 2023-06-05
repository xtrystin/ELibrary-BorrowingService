using ELibrary_BorrowingService.Domain.Exception;

namespace ELibrary_BorrowingService.Domain.Entity;
public class BorrowingHistory
{
    public int? Id { get; private set; }    // nullable -> DB will automatically create ID
    private DateTime _borrowedDate;
    private DateTime? _returnedDate;


    public int BookId { get; set; }
    public string CustomerId { get; set; }
    
    public Book Book { get; private set; } = null;
    public Customer Customer { get; private set; } = null;

    protected BorrowingHistory()
    {
    }

    public BorrowingHistory(Book book, Customer customer)
    {
        _borrowedDate = DateTime.UtcNow;
        _returnedDate = null;

        Book = book;
        BookId = book.Id;
        Customer = customer;
        CustomerId = customer.Id;
    }


    public bool IsOverTimeReturn() => _borrowedDate.AddDays(Book.MaxBorrowDays) < DateTime.Now;
    public int GetOverTimeDays()
    {
        if (IsOverTimeReturn() is false)
            return 0;
        
        var overtimeDays = DateTime.Now.Subtract(_borrowedDate.AddDays(Book.MaxBorrowDays)).Days;
        return overtimeDays;
    }

    public void Return()
    {
        if (IsReturned())
            throw new System.Exception($"Book {BookId} has been already returned");
        
        _returnedDate = DateTime.UtcNow;
    }

    public bool IsReturned() => _returnedDate is not null;
}
