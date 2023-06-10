using ELibrary_BorrowingService.Domain.Entity;
using System.Reflection;

namespace ELibrary_BorrowingService.Tests;

public class Tests
{
    [TestFixture]
    public class BookTests
    {
        [Test]
        public void Borrow_ThrowsException_WhenBookIsAlreadyBorrowed()
        {
            // Arrange
            var book = new Book(1, 5);
            var customer = new Customer("customer1", 5, 5);
            var borrowingHistory = new BorrowingHistory(book, customer);
            book.Borrow(borrowingHistory);

            // Act & Assert
            Assert.Throws<System.Exception>(() => book.Borrow(borrowingHistory));
        }

        [Test]
        public void Borrow_ThrowsException_WhenBookIsAlreadyBooked()
        {
            // Arrange
            var book = new Book(1, 5);
            var customer = new Customer("customer1", 5, 5);
            var bookingHistory = new BookingHistory(book, customer);
            book.BookBook(bookingHistory);

            // Act & Assert
            Assert.Throws<BorrowAfterBookingException>(() => book.Borrow(new BorrowingHistory(book, customer)));
        }

        [Test]
        public void Borrow_ThrowsException_WhenNoAvailableBooks()
        {
            // Arrange
            var book = new Book(1, 0);
            var customer = new Customer("customer1", 5, 5);

            // Act & Assert
            Assert.Throws<BookNotAvailableException>(() => book.Borrow(new BorrowingHistory(book, customer)));
        }

        [Test]
        public void Borrow_DecreasesAvailableBooksAndAddsToBorrowingHistory()
        {
            // Arrange
            var book = new Book(1, 5);
            var customer = new Customer("customer1", 5, 5);
            var borrowingHistory = new BorrowingHistory(book, customer);
            var availableBooksField = typeof(Book).GetField("_availabieBooks", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            book.Borrow(borrowingHistory);

            // Assert
            Assert.AreEqual(4, (int)availableBooksField.GetValue(book));
            Assert.Contains(borrowingHistory, (System.Collections.ICollection?)book.BorrowingHistory);
        }

        [Test]
        public void Return_ThrowsException_WhenBorrowingEntryNotFound()
        {
            // Arrange
            var book = new Book(1, 5);
            var customer = new Customer("customer1", 5, 5);

            // Act & Assert
            Assert.Throws<NoItemException>(() => book.Return("customer1"));
        }

        [Test]
        public void Return_IncreasesAvailableBooksAndMarksBorrowingEntryAsReturned()
        {
            // Arrange
            var book = new Book(1, 5);
            var customer = new Customer("customer1", 5, 5);
            var borrowingHistory = new BorrowingHistory(book, customer);
            var availableBooksField = typeof(Book).GetField("_availabieBooks", BindingFlags.NonPublic | BindingFlags.Instance);
            var t = typeof(Book);
            t.GetProperty("MaxBorrowDays").SetValue(book, 10);
            book.Borrow(borrowingHistory);

            // Act
            book.Return("customer1");

            // Assert
            Assert.AreEqual(5, (int)availableBooksField.GetValue(book));
            Assert.IsTrue(borrowingHistory.IsReturned());
        }

        [Test]
        public void Return_ThrowsException_WhenOverTimeReturn()
        {
            // Arrange
            var book = new Book(1, 5);
            var customer = new Customer("customer1", 5, 5);
            var borrowingHistory = new BorrowingHistory(book, customer);
            var t = typeof(Book);
            t.GetProperty("MaxBorrowDays").SetValue(book, -1);
            book.Borrow(borrowingHistory);

            // Act & Assert
            Assert.Throws<OverTimeReturnException>(() => book.Return("customer1"));
        }

        [Test]
        public void BookBook_ThrowsException_WhenBookIsAlreadyBooked()
        {
            // Arrange
            var book = new Book(1, 5);
            var customer = new Customer("customer1", 5, 5);
            var bookingHistory = new BookingHistory(book, customer);
            book.BookBook(bookingHistory);

            // Act & Assert
            Assert.Throws<System.Exception>(() => book.BookBook(bookingHistory));
        }

        [Test]
        public void BookBook_ThrowsException_WhenNoAvailableBooks()
        {
            // Arrange
            var book = new Book(1, 0);
            var customer = new Customer("customer1", 5, 5);
            var bookingHistory = new BookingHistory(book, customer);

            // Act & Assert
            Assert.Throws<BookNotAvailableException>(() => book.BookBook(bookingHistory));
        }

        [Test]
        public void BookBook_DecreasesAvailableBooksAndAddsToBookingHistory()
        {
            // Arrange
            var book = new Book(1, 5);
            var customer = new Customer("customer1", 5, 5);
            var bookingHistory = new BookingHistory(book, customer);
            var availableBooksField = typeof(Book).GetField("_availabieBooks", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            book.BookBook(bookingHistory);

            // Assert
            Assert.AreEqual(4, (int)availableBooksField.GetValue(book));
            Assert.Contains(bookingHistory, (System.Collections.ICollection?)book.BookingHistory);
        }

        [Test]
        public void UnBook_ThrowsException_WhenBookingEntryNotFound()
        {
            // Arrange
            var book = new Book(1, 5);
            var customer = new Customer("customer1", 5, 5);

            // Act & Assert
            Assert.Throws<NoItemException>(() => book.UnBook("customer1"));
        }

        [Test]
        public void UnBook_IncreasesAvailableBooksAndMarksBookingEntryAsUnbooked()
        {
            // Arrange
            var book = new Book(1, 5);
            var customer = new Customer("customer1", 5, 5);
            var bookingHistory = new BookingHistory(book, customer);
            var availableBooksField = typeof(Book).GetField("_availabieBooks", BindingFlags.NonPublic | BindingFlags.Instance);
            book.BookBook(bookingHistory);

            // Act
            book.UnBook("customer1");

            // Assert
            Assert.AreEqual(5, (int)availableBooksField.GetValue(book));
            Assert.IsFalse(bookingHistory.IsActive());
        }

        [Test]
        public void ChangeBookAvailability_IncreasesAvailableBooks()
        {
            // Arrange
            var book = new Book(1, 5);
            var availableBooksField = typeof(Book).GetField("_availabieBooks", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            book.ChangeBookAvailability(3);

            // Assert
            Assert.AreEqual(8, (int)availableBooksField.GetValue(book));
        }

        [Test]
        public void RemoveBook_SetsAvailableBooksToMinusOne()
        {
            // Arrange
            var book = new Book(1, 5);
            var availableBooksField = typeof(Book).GetField("_availabieBooks", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            book.RemoveBook();

            // Assert
            Assert.AreEqual(-1, (int)availableBooksField.GetValue(book));
        }
    }
}