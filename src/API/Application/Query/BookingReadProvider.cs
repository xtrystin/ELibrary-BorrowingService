using Dapper;
using ELibrary_BorrowingService.Application.Query.Model;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace ELibrary_BorrowingService.Application.Query;

public class BookingReadProvider : IBookingReadProvider
{
    private readonly IConfiguration _configuration;

    public BookingReadProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<List<BookingReadModel>> GetByCustomer(string customerId)
    {
        using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresResourceDb"));
        var sql = @$"select bh.""Id"", bh.""BookingDate"", bh.""BookingLimitDate"", bh.""IsActive"", bh.""IsSuccessful"", b.""Id"", b.""Title"", b.""ImageUrl""
            from ""borrowingService"".""BookingHistory"" bh 
            join ""bookService"".""Book"" b on b.""Id"" = bh.""BookId"" 
            where bh.""CustomerId"" = @CustomerId
";

        var bookings = await connection.QueryAsync<BookingReadModel, BookBasicInfoReadModel,
            (BookingReadModel BookingReadModel, BookBasicInfoReadModel BookBasicInfoReadModel)>(sql, (booking, book) => (booking, book), new { CustomerId = customerId }, splitOn: "Id");

        var result = bookings.GroupBy(bbu => bbu.BookingReadModel.Id)
            .Select(g =>
            {
                var booking = g.First().BookingReadModel;

                booking.BookBasicinfo = g.Select(bbu => bbu.BookBasicInfoReadModel).Where(x => x != null).GroupBy(x => x.Id)
                .Select(x => new BookBasicInfoReadModel(x.First().Id, x.First().Title, x.First().ImageUrl)).First();

                return booking;
            });

        return result.ToList();
    }

    public async Task<List<BookingReadModel>> GetByBook(int bookId)
    {
        using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresResourceDb"));
        var sql = @$"select bh.""Id"", bh.""BookingDate"", bh.""BookingLimitDate"", bh.""IsActive"", bh.""IsSuccessful"", u.""Id"", u.""FirstName"", u.""LastName"", anu.""Email""
            from ""borrowingService"".""BookingHistory"" bh 
            join public.""AspNetUsers"" anu on anu.""Id"" = bh.""CustomerId""
            join ""userService"".""User"" u on u.""Id"" = bh.""CustomerId"" 
            where bh.""BookId"" = @BookId
";

        var bookings = await connection.QueryAsync<BookingReadModel, CustomerBasicInfoReadModel,
            (BookingReadModel BookingReadModel, CustomerBasicInfoReadModel CustomerBasicInfoReadModel)>(sql, (booking, user) => (booking, user), new { BookId = bookId }, splitOn: "Id");

        var result = bookings.GroupBy(bbu => bbu.BookingReadModel.Id)
            .Select(g =>
            {
                var booking = g.First().BookingReadModel;

                booking.CustomerBasicinfo = g.Select(bbu => bbu.CustomerBasicInfoReadModel).Where(x => x != null).GroupBy(x => x.Id)
                .Select(x => new CustomerBasicInfoReadModel(x.First().Id, x.First().FirstName, x.First().LastName, x.First().Email)).First();

                return booking;
            });

        return result.ToList();
    }

    public async Task<List<BookingReadModel>> GetAll()
    {
        using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresResourceDb"));
        var sql = @$"select bh.""Id"", bh.""BookingDate"", bh.""BookingLimitDate"", bh.""IsActive"", bh.""IsSuccessful"", b.""Id"", b.""Title"", b.""ImageUrl"", u.""Id"", u.""FirstName"", u.""LastName"", anu.""Email""
            from ""borrowingService"".""BookingHistory"" bh 
            join ""bookService"".""Book"" b on b.""Id"" = bh.""BookId"" 
            join public.""AspNetUsers"" anu on anu.""Id"" = bh.""CustomerId""
            join ""userService"".""User"" u on u.""Id"" = bh.""CustomerId"" 
";

        var bookings = await connection.QueryAsync<BookingReadModel, BookBasicInfoReadModel, CustomerBasicInfoReadModel,
            (BookingReadModel BookingReadModel, BookBasicInfoReadModel BookBasicInfoReadModel, CustomerBasicInfoReadModel CustomerBasicInfoReadModel)>(sql, (booking, book, user) => (booking, book, user), splitOn: "Id");

        var result = bookings.GroupBy(bbu => bbu.BookingReadModel.Id)
            .Select(g =>
            {
                var booking = g.First().BookingReadModel;

                booking.BookBasicinfo = g.Select(bbu => bbu.BookBasicInfoReadModel).Where(x => x != null).GroupBy(x => x.Id)
                .Select(x => new BookBasicInfoReadModel(x.First().Id, x.First().Title, x.First().ImageUrl)).First();

                booking.CustomerBasicinfo = g.Select(bbu => bbu.CustomerBasicInfoReadModel).Where(x => x != null).GroupBy(x => x.Id)
                .Select(x => new CustomerBasicInfoReadModel(x.First().Id, x.First().FirstName, x.First().LastName, x.First().Email)).First();

                return booking;
            });

        return result.ToList();
    }
}