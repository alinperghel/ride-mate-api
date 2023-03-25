using Microsoft.EntityFrameworkCore;
using RideMateApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<RideMateDbContext>(opt => opt.UseInMemoryDatabase("RideMate"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/", async (RideMateDbContext db) => {
    var user = new User();
    user.Email = "client@ridemate.com";
    user.Name = "Client";
    user.Password = "parola";

    var driver = new User();
    driver.Email = "driver@ridemate.com";
    driver.Name = "Driver";
    driver.Password = "parola";

    db.Users.Add(user);
    db.Users.Add(driver);

    var ride1 = new Ride();
    ride1.AvailableSeats = 1;
    ride1.DriverId = driver.Id;
    ride1.SourceId = 1;
    ride1.DestinationId = 3;

    db.Rides.Add(ride1);

    await db.SaveChangesAsync();

    return Results.Ok("Hello world!");
});

app.MapGet("/rides", async (RideMateDbContext db) =>
    await db.Rides.ToListAsync());

app.MapPost("/rides/search", async (RideSearchInput rideInput, RideMateDbContext db) =>
{
    return await db.Rides.Where(t => t.DestinationId == rideInput.DestinationId)
            .Where(t => t.SourceId == rideInput.SourceId)
            .Where(t => t.AvailableSeats > 0)
            .Where(t => t.Date > rideInput.Date).ToListAsync();
});

app.MapPost("/rides", async (Ride rideInput, RideMateDbContext db) =>
{
    db.Rides.Add(rideInput);
    await db.SaveChangesAsync();

    return Results.Created($"/rides/{rideInput.Id}", rideInput);
});

app.MapPost("/register", async (User userInput, RideMateDbContext db) =>
{
    db.Users.Add(userInput);
    await db.SaveChangesAsync();

    return Results.Created($"/user/{userInput.Id}", userInput);
});

app.MapPost("/login", (User userInput, RideMateDbContext db) =>
{
    User user;
    try {
        user = db.Users.Where(t => t.Email == userInput.Email).First();
    } catch {
        return Results.NotFound();
    }
    

    if (user.Password != userInput.Password)
    {
        return Results.NotFound();
    }

    return Results.Ok(user);
});

app.MapGet("/rides/{id}", async (int id, RideMateDbContext db) =>
    await db.Rides.FindAsync(id)
        is Ride ride
            ? Results.Ok(ride)
            : Results.NotFound());

app.MapPost("/rides/{id}/book", async (int rideId, UserBookInput userBookInput, RideMateDbContext db) =>
{
    var ride = await db.Rides.FindAsync(rideId);

    if (ride == null) {
        return Results.NotFound();
    }

    if (ride.AvailableSeats > 0) {
        ride.AvailableSeats = ride.AvailableSeats - 1;
    } else {
        return Results.NotFound();

    }

    var ridePassager = new RidePassager();

    ridePassager.PassagerId = userBookInput.UserId;
    ridePassager.RideId = rideId;

    db.RidePassagers.Add(ridePassager);

    await db.SaveChangesAsync();

    return Results.Ok();
});

app.MapGet("/ride/bookings/{id}", async (int rideId, RideMateDbContext db) =>
    await db.RidePassagers.Where(t => t.RideId == rideId).ToListAsync()
);

app.MapGet("/user/bookings/{id}", async (int userId, RideMateDbContext db) =>
    await db.RidePassagers.Where(t => t.PassagerId == userId).ToListAsync()
);

app.Run();