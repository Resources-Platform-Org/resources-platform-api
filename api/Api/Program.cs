using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models; // added

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(); // keeps minimal OpenAPI support

// Register MVC controllers so actions are discovered
builder.Services.AddControllers();
// Explorer for endpoint metadata used by Swagger/Swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Add Swashbuckle Swagger generation
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Resources Platform API",
        Version = "v1",
        Description = "API documentation for Resources Platform",
    });
});

var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(ConnectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Resources Platform API v1");
    });

    app.MapOpenApi(); // existing minimal openapi mapping (optional)
}

app.UseHttpsRedirection();

// Map controller endpoints
app.MapControllers();

app.Run();

