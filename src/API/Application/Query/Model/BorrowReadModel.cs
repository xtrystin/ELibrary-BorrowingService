namespace ELibrary_BorrowingService.Application.Query.Model;

public class BorrowReadModel
{
    public int Id { get; set; }
    public DateTime BorrowedDate { get; set; }
    public DateTime? ReturnedDate { get; set; }
    public BookBasicInfoReadModel BookBasicinfo { get; set; }
    public CustomerBasicInfoReadModel CustomerBasicinfo { get; set; }
}
