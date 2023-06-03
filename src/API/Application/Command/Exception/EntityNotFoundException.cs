namespace ELibrary_BorrowingService.Application.Command.Exception;

public class EntityNotFoundException : System.Exception
{
    public EntityNotFoundException(string message) : base(message)
    {
    }
}
