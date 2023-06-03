namespace ELibrary_BorrowingService.Domain.Exception;
public class TooManyBooksException : System.Exception
{
	public TooManyBooksException(string message) : base(message)
	{
	}
}
