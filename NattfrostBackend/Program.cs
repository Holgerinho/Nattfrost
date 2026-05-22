using Microsoft.EntityFrameworkCore;
using NattfrostBackend.Data;
using NattfrostBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient<OpenMeteoService>();
// !!! Here is the place Holgerino where you swap NoOpEmailService for your real email service
builder.Services.AddSingleton<IEmailNotificationService, NoOpEmailService>();
builder.Services.AddSingleton<FrostCheckBackgroundService>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<FrostCheckBackgroundService>());
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

// HENRIKS TEMP TEST TO CHECK SERVICES! 
// For future references n shid använd: service.CityExistsAsync("NAMNPÅSTADEN"); för att testa om den finns eller ej när vi reggar en subskrajber
// TYp så här: if (!await _openMeteo.CityExistsAsync(request.City))
//             return BadRequest(new { error = $"City '{request.City}' was not found. Check the spelling." });


/*
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider.GetRequiredService<OpenMeteoService>();

    Console.WriteLine("=== CityExistsAsync ===");
    Console.WriteLine($"Gothenburg: {await service.CityExistsAsync("Gothenburg")}");
    Console.WriteLine($"asdfxyz:    {await service.CityExistsAsync("asdfxyz")}");

    Console.WriteLine("=== GetCoordinatesAsync ===");
    var coords = await service.GetCoordinatesAsync("Gothenburg");
    Console.WriteLine($"{coords.CityName} -> {coords.Latitude}, {coords.Longitude}");

    Console.WriteLine("=== HasFrostRiskAsync ===");
    var frost = await service.HasFrostRiskAsync(coords.Latitude, coords.Longitude);
    Console.WriteLine($"Frost risk: {frost}");
}
*/

app.Run();
