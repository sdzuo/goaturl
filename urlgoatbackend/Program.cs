using Microsoft.EntityFrameworkCore;
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
            .WithOrigins("https://localhost:4200") // Specify allowed origins for frontend
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Configure the database context
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Configure Dependency Injection
builder.Services.AddTransient<Seed>();
builder.Services.AddScoped<IUrlMappingRepository, UrlMappingRepository>();

var app = builder.Build();

// Seed the database if 'seeddata' argument is provided
if (args.Length == 1 && args[0].ToLower() == "seeddata")
    SeedData(app);

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<Seed>();
        service.SeedData();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Enable CORS
app.UseCors("AllowMultipleOrigins");

app.MapControllers();

app.Run();
