namespace ELibrary_BorrowingService.Application.Query.Model;

public class BookingReadModel
{
    public int Id { get; set; }
    public DateTime BookingDate { get; set; }
    public DateTime BookingLimitDate { get; set; }
    public bool IsActive { get; set; }
    public bool? IsSuccessful { get; set; }
    public BookBasicInfoReadModel BookBasicinfo { get; set; }
    public CustomerBasicInfoReadModel CustomerBasicinfo { get; set; }
}
