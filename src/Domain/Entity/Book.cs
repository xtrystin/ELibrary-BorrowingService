using ELibrary_BorrowingService.Domain.Exception;
using static System.Reflection.Metadata.BlobBuilder;

namespace ELibrary_BorrowingService.Domain.Entity;
public class Book
{
    public int Id { get; private set; }
    private int _availabieBooks;
    private decimal _penaltyAmount;
    public int MaxBorrowDays { get; private set; }
    public int MaxBookingDays { get; private set; }

    private List<BorrowingHistory> _borrowingHistory;
    private List<BookingHistory> _bookingHistory;

    public IReadOnlyCollection<BookingHistory> BookingHistory => _bookingHistory;
    public IReadOnlyCollection<BorrowingHistory> BorrowingHistory => _borrowingHistory;

    protected Book()
    {
    }
    public Book(int id, int availableBooks)
    {
        Id = id;
        _availabieBooks = availableBooks;
    }

    public bool IsAvailable() => _availabieBooks > 0;

    public void Borrow(BorrowingHistory borrowing)
    {
        var activeBooking = _bookingHistory.FirstOrDefault(x => x.IsActive());
        if (activeBooking is not null)
        {
            borrowing.Customer.Borrow();
            activeBooking.Unbook(unBookAfterBorrow: true);
            _borrowingHistory.Add(borrowing);

            throw new BorrowAfterBookingException();
        }
        else if (_availabieBooks <= 0)
        {
            throw new BookNotAvailableException(Id);
        }
        else
        {
            borrowing.Customer.Borrow();
            _availabieBooks--;
            _borrowingHistory.Add(borrowing);
        }

    }

    public void Return(string customerId) 
    {
        var borrowing = _borrowingHistory.FirstOrDefault(x => x.CustomerId == customerId
            && x.IsReturned() is false);
        if (borrowing is null)
            throw new NoItemException($"Book {Id} does not have this borrowing entry");

        borrowing.Return();
        _availabieBooks++;
        borrowing.Customer.Return();

        if (borrowing.IsOverTimeReturn())
        {
            var overtimeDays = borrowing.GetOverTimeDays();
            var penalty = _penaltyAmount * overtimeDays;
            throw new OverTimeReturnException(penalty);
        }
    }

    public void BookBook(BookingHistory booking)
    {
        if (_availabieBooks <= 0)
            throw new BookNotAvailableException(Id);

        booking.Customer.Book();
        _bookingHistory.Add(booking);
        _availabieBooks--;
    }

    public void UnBook(string customerId) 
    {
        var booking = _bookingHistory.FirstOrDefault(x => x.CustomerId == customerId
            && x.IsActive());
        if (booking is null)
            throw new NoItemException($"Book {Id} does not have this booking entry");

        booking.Unbook(unBookAfterBorrow: false);
        _availabieBooks++;
        booking.Customer.UnBook();
    }

    public void ChangeBookAvailability(int amount) => _availabieBooks += amount;

    public void RemoveBook() => _availabieBooks = -1;
}
