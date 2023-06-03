namespace ELibrary_BorrowingService.Domain.Exception;
public class OverTimeReturnException : System.Exception
{
	public OverTimeReturnException(decimal penaltyAmount) : base(penaltyAmount.ToString())
	{
	}
}
