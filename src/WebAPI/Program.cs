<<<<<<< Updated upstream
using Application.Interfaces;
=======
using Application;
using Infrastructure;
>>>>>>> Stashed changes
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
<<<<<<< Updated upstream
using Microsoft.Extensions.DependencyInjection;
=======
using WebAPI.Middleware;
>>>>>>> Stashed changes

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

<<<<<<< Updated upstream
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
=======
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddCors(o => o.AddPolicy("Angular", p =>
    p.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod()));
>>>>>>> Stashed changes

// 2. Inversion de dépendance (IoC) pour les repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
var app = builder.Build();

<<<<<<< Updated upstream

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
=======
app.UseExceptionHandler();
app.UseStatusCodePages();   // corps ProblemDetails pour les 404/405 sans contenu
if (app.Environment.IsDevelopment())
    app.MapOpenApi();
>>>>>>> Stashed changes

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

// Exécution du Seeding au démarrage
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await DbInitializer.SeedAsync(context);
}

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
