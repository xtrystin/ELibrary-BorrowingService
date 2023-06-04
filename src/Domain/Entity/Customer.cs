using ELibrary_BorrowingService.Domain.Exception;

namespace ELibrary_BorrowingService.Domain.Entity;
public class Customer
{
    public string Id { get; private set; }
    private bool _isAccountBlocked;
    private int _currentBookedBookNr;
    private int _currentBorrowedBookNr;
    private int _maxBooksToBook;
    private int _maxBooksToBorrow;


    private List<BookingHistory> _bookingHistory = new();
    private List<BorrowingHistory> _borrowingHistory = new();

    public IReadOnlyCollection<BookingHistory> BookingHistory => _bookingHistory;
    public IReadOnlyCollection<BorrowingHistory> BorrowingHistory => _borrowingHistory;

    protected Customer()
    {
    }

    public Customer(string id, int maxBooksToBook, int maxBooksToBorrow)
    {
        Id = id;
        _isAccountBlocked= false;
        _maxBooksToBook = maxBooksToBook;
        _maxBooksToBorrow = maxBooksToBorrow;
    }

    public void Book()
    {
        if (_isAccountBlocked is false && _currentBookedBookNr < _maxBooksToBook)
            throw new TooManyBooksException("You cannot book more books");
        _currentBookedBookNr++;
    }
    public void UnBook() => _currentBookedBookNr--;

    public void Borrow()
    {
        if(_isAccountBlocked is false && _currentBorrowedBookNr < _maxBooksToBorrow)
            throw new TooManyBooksException("You cannot book more books");
        _currentBorrowedBookNr++;
    }
    public void Return() => _currentBorrowedBookNr--;

    public void ChangeAccountStatus(bool isAccountBlocked) 
        => _isAccountBlocked = isAccountBlocked;

    public void DeleteCustomer() => (_maxBooksToBook, _maxBooksToBorrow) = (-1, -1);
}
