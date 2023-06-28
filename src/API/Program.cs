using ELibrary_BorrowingService.RabbitMq;
using ELibrary_BorrowingService.Application;
using ELibrary_BorrowingService.Extensions;
using ELibrary_BorrowingService.Infrastructure.EF;
using ELibrary_BorrowingService.Jobs;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenCorsPolicy();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddPostgres(builder.Configuration);
builder.Services.AddServiceBus(builder.Configuration);

builder.Services.AddProviderCollection();
builder.Services.AddQuartzJobs(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler("/error");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("OpenCorsPolicy");

app.UseMetricServer();
app.UseHttpMetrics(options => options.AddCustomLabel("host", context => context.Request.Host.Host));

app.UseAuthorization();

app.MapControllers();

app.Run();
