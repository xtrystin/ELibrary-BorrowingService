namespace ELibrary_BorrowingService.Application.Query.Model;

public class BookBasicInfoReadModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ImageUrl { get; set; }

    public BookBasicInfoReadModel(int id, string title, string imageUrl)
    {
        Id = id;
        Title = title;
        ImageUrl = imageUrl;
    }
}