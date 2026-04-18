using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using urlgoatbackend;
using urlgoatbackend.Data;
using urlgoatbackend.Interfaces;
using urlgoatbackend.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Enable Cross-Origin Resource Sharing (CORS)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMultipleOrigins",
        builder => builder
            .WithOrigins("http://localhost:4200") // Specify allowed origins for frontend
            .AllowAnyHeader()
            .AllowAnyMethod());
});

//adding health checks for when we dockerize
builder.Services.AddHealthChecks();  

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Configure the database context
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure());
});

// Configure Dependency Injection
builder.Services.AddTransient<Seed>();
builder.Services.AddScoped<IUrlMappingRepository, UrlMappingRepository>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

await ApplyMigrationsAsync(app);

app.MapHealthChecks("/health"); //mapping health checks

// Seed the database if 'seeddata' argument is provided
if (args.Length == 1 && args[0].ToLower() == "seeddata")
    SeedData(app);

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetRequiredService<Seed>();
        service.SeedData();
    }
}

// Configure the HTTP request pipeline.
/* if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
} */

app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();
//app.UseAuthorization();

// Enable CORS
app.UseCors("AllowMultipleOrigins");

app.MapControllers();

app.Run();

static async Task ApplyMigrationsAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    var logger = scope.ServiceProvider
        .GetRequiredService<ILoggerFactory>()
        .CreateLogger("Startup");

    const int maxAttempts = 10;

    for (var attempt = 1; attempt <= maxAttempts; attempt++)
    {
        try
        {
            await context.Database.MigrateAsync();
            logger.LogInformation("Database migrations applied successfully.");
            return;
        }
        catch (Exception ex) when (attempt < maxAttempts)
        {
            logger.LogWarning(
                ex,
                "Database migration attempt {Attempt}/{MaxAttempts} failed. Retrying...",
                attempt,
                maxAttempts);

            await Task.Delay(TimeSpan.FromSeconds(5));
        }
    }

    await context.Database.MigrateAsync();
}
