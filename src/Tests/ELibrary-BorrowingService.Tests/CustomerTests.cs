using System.Reflection;

namespace ELibrary_BorrowingService.Tests;

[TestFixture]
public class CustomerTests
{
    [Test]
    public void Book_ThrowsException_WhenAccountIsBlockedAndReachedMaxBooksToBook()
    {
        // Arrange
        var customer = new Customer("customer1", 5, 5);
        customer.ChangeAccountStatus(true);

        // Act & Assert
        Assert.Throws<TooManyBooksException>(() => customer.Book());
    }

    [Test]
    public void Book_IncreasesCurrentBookedBookNr()
    {
        // Arrange
        var customer = new Customer("customer1", 5, 5);
        var currentBookedBookNrField = typeof(Customer).GetField("_currentBookedBookNr", BindingFlags.NonPublic | BindingFlags.Instance);

        // Act
        customer.Book();

        // Assert
        Assert.AreEqual(1, (int)currentBookedBookNrField.GetValue(customer));
    }

    [Test]
    public void UnBook_DecreasesCurrentBookedBookNr()
    {
        // Arrange
        var customer = new Customer("customer1", 5, 5);
        customer.Book();
        var currentBookedBookNrField = typeof(Customer).GetField("_currentBookedBookNr", BindingFlags.NonPublic | BindingFlags.Instance);

        // Act
        customer.UnBook();

        // Assert
        Assert.AreEqual(0, (int)currentBookedBookNrField.GetValue(customer));
    }

    [Test]
    public void Borrow_ThrowsException_WhenAccountIsBlockedAndReachedMaxBooksToBorrow()
    {
        // Arrange
        var customer = new Customer("customer1", 5, 5);
        customer.ChangeAccountStatus(true);

        // Act & Assert
        Assert.Throws<TooManyBooksException>(() => customer.Borrow(false));
    }

    [Test]
    public void Borrow_IncreasesCurrentBorrowedBookNr()
    {
        // Arrange
        var customer = new Customer("customer1", 5, 5);
        var currentBorrowedBookNrField = typeof(Customer).GetField("_currentBorrowedBookNr", BindingFlags.NonPublic | BindingFlags.Instance);

        // Act
        customer.Borrow(false);

        // Assert
        Assert.AreEqual(1, (int)currentBorrowedBookNrField.GetValue(customer));
    }

    [Test]
    public void Return_DecreasesCurrentBorrowedBookNr()
    {
        // Arrange
        var customer = new Customer("customer1", 5, 5);
        customer.Borrow(false);
        var currentBorrowedBookNrField = typeof(Customer).GetField("_currentBorrowedBookNr", BindingFlags.NonPublic | BindingFlags.Instance);

        // Act
        customer.Return();

        // Assert
        Assert.AreEqual(0, (int)currentBorrowedBookNrField.GetValue(customer));
    }
}