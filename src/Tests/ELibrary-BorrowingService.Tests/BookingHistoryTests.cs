namespace ELibrary_BorrowingService.Tests;

[TestFixture]
public class BookingHistoryTests
{
    [Test]
    public void Unbook_MarksBookingAsUnbooked()
    {
        // Arrange
        var book = new Book(1, 5);
        var customer = new Customer("customer1", 5, 5);
        var bookingHistory = new BookingHistory(book, customer);

        // Act
        bookingHistory.Unbook(false);

        // Assert
        Assert.IsFalse(bookingHistory.IsActive());
    }
}

