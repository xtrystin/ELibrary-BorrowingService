namespace ELibrary_BorrowingService.Domain.Entity;
public class BookingHistory
{
    public int? Id { get; private set; }    // nullable -> DB will automatically create ID
    private DateTime _bookingDate;
    private DateTime _bookingLimitDate;
    private bool _isActive;
    private bool? _isSuccessful;

    public int BookId { get; set; }
    public string CustomerId { get; set; }

    public Book Book { get; private set; } = null;
    public Customer Customer { get; private set; } = null;

    protected BookingHistory()
    {
    }

    public BookingHistory(Book book, Customer customer)
    {
        _bookingDate = DateTime.UtcNow;
        _isSuccessful = null;

        _bookingLimitDate = _bookingDate.AddDays(book.MaxBookingDays);
        _isActive = true;
        Book = book;
        BookId = book.Id;
        Customer = customer;
        CustomerId = customer.Id;
    }

    public void Unbook(bool unBookAfterBorrow)
    {
        if (_isActive is false)
            throw new System.Exception($"Booking {Id} is not valid");

        _isSuccessful = unBookAfterBorrow;
        _isActive = false;
    }

    public bool IsActive() => _isActive;
}
