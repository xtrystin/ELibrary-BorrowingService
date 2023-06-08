using Dapper;
using ELibrary_BorrowingService.Application.Command.Model;
using ELibrary_BorrowingService.Application.Query.Model;
using Npgsql;

namespace ELibrary_BorrowingService.Application.Query;

public class BorrowReadProvider : IBorrowReadProvider
{
    private readonly IConfiguration _configuration;

    public BorrowReadProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<List<BorrowReadModel>> GetByCustomer(string customerId)
    {
        using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresResourceDb"));
        var sql = @$"select bh.""Id"", bh.""BorrowedDate"", bh.""ReturnedDate"", b.""Id"", b.""Title"", b.""ImageUrl""
            from ""borrowingService"".""BorrowingHistory"" bh 
            join ""bookService"".""Book"" b on b.""Id"" = bh.""BookId"" 
            where bh.""CustomerId"" = @CustomerId
";

        var borrows = await connection.QueryAsync<BorrowReadModel, BookBasicInfoReadModel,
            (BorrowReadModel BorrowReadModel, BookBasicInfoReadModel BookBasicInfoReadModel)>(sql, (borrow, book) => (borrow, book), new { CustomerId = customerId }, splitOn: "Id");

        var result = borrows.GroupBy(bbu => bbu.BorrowReadModel.Id)
            .Select(g =>
            {
                var borrow = g.First().BorrowReadModel;

                borrow.BookBasicinfo = g.Select(bbu => bbu.BookBasicInfoReadModel).Where(x => x != null).GroupBy(x => x.Id)
                .Select(x => new BookBasicInfoReadModel(x.First().Id, x.First().Title, x.First().ImageUrl)).First();

                return borrow;
            });

        return result.ToList();
    }

    public async Task<List<BorrowReadModel>> GetByBook(int bookId)
    {
        using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresResourceDb"));
        var sql = @$"select bh.""Id"", bh.""BorrowedDate"", bh.""ReturnedDate"", u.""Id"", u.""FirstName"", u.""LastName"", anu.""Email""
            from ""borrowingService"".""BorrowingHistory"" bh 
            join public.""AspNetUsers"" anu on anu.""Id"" = bh.""CustomerId""
            join ""userService"".""User"" u on u.""Id"" = bh.""CustomerId"" 
            where bh.""BookId"" = @BookId
";

        var borrows = await connection.QueryAsync<BorrowReadModel, CustomerBasicInfoReadModel,
            (BorrowReadModel BorrowReadModel, CustomerBasicInfoReadModel CustomerBasicInfoReadModel)>(sql, (borrow, user) => (borrow, user), new { BookId = bookId }, splitOn: "Id");

        var result = borrows.GroupBy(bbu => bbu.BorrowReadModel.Id)
            .Select(g =>
            {
                var borrow = g.First().BorrowReadModel;

                borrow.CustomerBasicinfo = g.Select(bbu => bbu.CustomerBasicInfoReadModel).Where(x => x != null).GroupBy(x => x.Id)
                .Select(x => new CustomerBasicInfoReadModel(x.First().Id, x.First().FirstName, x.First().LastName, x.First().Email)).First();

                return borrow;
            });

        return result.ToList();
    }

    public async Task<List<BorrowReadModel>> GetAll()
    {
        using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresResourceDb"));
        var sql = @$"select bh.""Id"", bh.""BorrowedDate"", bh.""ReturnedDate"", b.""Id"", b.""Title"", b.""ImageUrl"", u.""Id"", u.""FirstName"", u.""LastName"", anu.""Email""
            from ""borrowingService"".""BorrowingHistory"" bh 
            join ""bookService"".""Book"" b on b.""Id"" = bh.""BookId"" 
            join public.""AspNetUsers"" anu on anu.""Id"" = bh.""CustomerId""
            join ""userService"".""User"" u on u.""Id"" = bh.""CustomerId"" 
";

        var borrows = await connection.QueryAsync<BorrowReadModel, BookBasicInfoReadModel, CustomerBasicInfoReadModel,
            (BorrowReadModel BorrowReadModel, BookBasicInfoReadModel BookBasicInfoReadModel, CustomerBasicInfoReadModel CustomerBasicInfoReadModel)>(sql, (borrow, book, user) => (borrow, book, user), splitOn: "Id");

        var result = borrows.GroupBy(bbu => bbu.BorrowReadModel.Id)
            .Select(g =>
            {
                var borrow = g.First().BorrowReadModel;

                borrow.BookBasicinfo = g.Select(bbu => bbu.BookBasicInfoReadModel).Where(x => x != null).GroupBy(x => x.Id)
                .Select(x => new BookBasicInfoReadModel(x.First().Id, x.First().Title, x.First().ImageUrl)).First();

                borrow.CustomerBasicinfo = g.Select(bbu => bbu.CustomerBasicInfoReadModel).Where(x => x != null).GroupBy(x => x.Id)
                .Select(x => new CustomerBasicInfoReadModel(x.First().Id, x.First().FirstName, x.First().LastName, x.First().Email)).First();

                return borrow;
            });

        return result.ToList();
    }
}
