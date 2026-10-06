using System.Text;
using System.Threading.RateLimiting;
using Application;
using Infrastructure;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>("databas









e");

// Limite les tentatives de connexion / inscription : 10 requêtes par minute et par adresse IP
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("auth", httpContext => RateLimitPartition.GetFixedWindowLimiter(
        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
        factory: _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 10,
            Window = TimeSpan.FromMinutes(1),
 











           QueueLimit = 0,
        }));
});

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt");










        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt["Issuer"],
            ValidateAudience = true,
 






           ValidAudience = jwt["Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1),
 




           NameClaimType = "name",
            RoleClaimType = "role",
        };
    });
builder.Services.AddAuthorization();






builder.Services.AddCors(o => o.AddPolicy("Angular", p =>
    p.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod()));

v



ar app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();





if (app.Environment.IsDevelopment())
    app.MapOpenApi();

a



pp.UseHttpsRedirection();
app.UseCors("Angular");
app.UseRateLimiter();
app.UseAuthentication();
a



pp.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");




using (var scope = app.Services.CreateScope())
{
 


   var sp = scope.ServiceProvider;
    var context = sp.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
 


   await DbInitializer.SeedAsync(context);
    await IdentitySeeder.SeedAsync(sp, builder.Configuration);
}




app.
Run();

