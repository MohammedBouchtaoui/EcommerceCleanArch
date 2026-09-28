using Domain.Interfaces;
using Infrastructure.Data; // Assure-toi que le namespace de ton ApplicationDbContext est correct
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Ajout de la base de données (DbContext)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Si tu utilises SQLite / PostgreSQL, adapte UseSqlServer() en UseSqlite() ou UseNpgsql()

// 2. Injection du Generic Repository (DIP)
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// 3. Configuration OpenAPI / Controllers
builder.Services.AddControllers(); // Utile si tu prévois d'utiliser des API Controllers
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure le pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Optionnel: Mappe tes contrôleurs si tu en ajoutes
app.MapControllers();

app.Run();