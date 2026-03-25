using Microsoft.EntityFrameworkCore;
using TandemBackend.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "database", "tandem.db");
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlite($"Data Source={dbPath}")
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) { }

app.MapOpenApi();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "Open API V1");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Update  Migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    dbContext.Database.Migrate();
}

app.Run();
