using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary_BorrowingService.Domain.Exception;
public class BookNotAvailableException : System.Exception
{
	public BookNotAvailableException(int bookId) : base($"Book {bookId} is not available")
	{

	}
}
