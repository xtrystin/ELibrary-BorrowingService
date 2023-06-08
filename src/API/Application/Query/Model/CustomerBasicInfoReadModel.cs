namespace ELibrary_BorrowingService.Application.Query.Model;

public class CustomerBasicInfoReadModel
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    public CustomerBasicInfoReadModel(string id, string firstName, string lastName, string email)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }
}
