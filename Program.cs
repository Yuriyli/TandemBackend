using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using TandemBackend.Data;
using TandemBackend.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer(
        (document, context, cancellationToken) =>
        {
            // Ensure instances exist
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes ??=
                new Dictionary<string, IOpenApiSecurityScheme>();

            document.Components.SecuritySchemes.Add(
                "bearer",
                new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter 'Bearer {token}'",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                }
            );

            document.SetReferenceHostDocument();

            return Task.CompletedTask;
        }
    );

    // Optional: Add operation transformer to conditionally add security
    options.AddOperationTransformer(
        (operation, context, cancellationToken) =>
        {
            // Check if the endpoint requires authorization
            var hasAuthorize = context.Description.ActionDescriptor.EndpointMetadata.Any(em =>
                em is Microsoft.AspNetCore.Authorization.AuthorizeAttribute
            );

            if (hasAuthorize)
            {
                operation.Security ??= new List<OpenApiSecurityRequirement>();
                operation.Security.Add(
                    new OpenApiSecurityRequirement
                    {
                        { new OpenApiSecuritySchemeReference("bearer"), [] },
                    }
                );
            }

            return Task.CompletedTask;
        }
    );
});

var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "database", "tandem.db");
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlite($"Data Source={dbPath}")
);

builder.Services.AddAuthorization();

// Set key from secret (optional)
AuthOptions.SetSeedKey(builder.Configuration["AuthOptions:Key"]);
builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.ISSUER,
            ValidateAudience = true,
            ValidAudience = AuthOptions.AUDIENCE,
            ValidateLifetime = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) { }

app.MapOpenApi();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "Open API V1");
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Update  Migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    dbContext.Database.Migrate();
}

app.Run();
