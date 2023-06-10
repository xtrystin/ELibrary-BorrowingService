using System.Reflection;

namespace ELibrary_BorrowingService.Tests;

[TestFixture]
public class BorrowingHistoryTests
{
    [Test]
    public void Return_ThrowsException_WhenBookAlreadyReturned()
    {
        // Arrange
        var book = new Book(1, 5);
        var customer = new Customer("customer1", 5, 5);
        var borrowingHistory = new BorrowingHistory(book, customer);
        borrowingHistory.Return(); // Mark as returned

        // Act & Assert
        Assert.Throws<System.Exception>(() => borrowingHistory.Return());
    }

    [Test]
    public void Return_MarksBookAsReturned()
    {
        // Arrange
        var book = new Book(1, 5);
        var customer = new Customer("customer1", 5, 5);
        var borrowingHistory = new BorrowingHistory(book, customer);

        // Act
        borrowingHistory.Return();

        // Assert
        Assert.IsTrue(borrowingHistory.IsReturned());
    }

    [Test]
    public void IsOverTimeReturn_ReturnsTrue_WhenBookIsReturnedAfterMaxBorrowDays()
    {
        // Arrange
        var book = new Book(1, 5);
        var customer = new Customer("customer1", 5, 5);
        var borrowingHistory = new BorrowingHistory(book, customer);
        borrowingHistory.Return(); // Mark as returned

        // Act
        var isOverTime = borrowingHistory.IsOverTimeReturn();

        // Assert
        Assert.IsTrue(isOverTime);
    }

    [Test]
    public void GetOverTimeDays_ReturnsCorrectNumberOfDays_WhenBookIsReturnedAfterMaxBorrowDays()
    {
        // Arrange
        var book = new Book(1, 5);
        var customer = new Customer("customer1", 5, 5);
        var borrowingHistory = new BorrowingHistory(book, customer);
        var borrowedDateField = typeof(BorrowingHistory).GetField("_borrowedDate", BindingFlags.NonPublic | BindingFlags.Instance);
        borrowingHistory.Return(); // Mark as returned

        // Act
        var overtimeDays = borrowingHistory.GetOverTimeDays();

        // Assert
        var expectedDays = (DateTime.Now - ((DateTime)borrowedDateField.GetValue(borrowingHistory)).AddDays(book.MaxBorrowDays)).Days;
        Assert.AreEqual(expectedDays, overtimeDays);
    }
}