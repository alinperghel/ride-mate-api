using Microsoft.EntityFrameworkCore;
using RideMateApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<RideMateDbContext>(opt => opt.UseInMemoryDatabase("RideMate"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();

