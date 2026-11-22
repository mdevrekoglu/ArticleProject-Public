using ArticleProject.Api;
using ArticleProject.Api.Application.Middleware;
using ArticleProject.Data;
using ArticleProject.Data.ArticleProjectDbContext;
using ArticleProject.Model;
using ArticleProject.Model.Models.CommonModels.DataTransferModels;
using ArticleProject.Resources;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
ConfigureServices(builder.Services, builder.Configuration);

// Build the application
var app = builder.Build();

// Apply migrations on startup
ApplyMigrations(app);

// Configure the middleware pipeline
ConfigureMiddleware(app);

app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    // CORS configuration
    var corsSettings = configuration.GetSection("CorsSettings").Get<CorsSettings>()!;
    services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", builder =>
        {
            builder.WithExposedHeaders("Content-Disposition")
                   .WithOrigins(corsSettings.Origins)
                   .WithMethods(corsSettings.Methods)
                   .AllowAnyHeader();
        });
    });

    // Add essential services (Controllers, Swagger, API, MediatR)
    services.AddControllers();
    services.AddApi(configuration);
    services.AddData();
    services.AddModel();
    services.AddResources();

    // Configure Swagger/OpenAPI
    services.AddEndpointsApiExplorer();

    services.AddSwaggerGen(options =>
    {
        // Add the JWT Bearer token configuration in Swagger
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter 'Bearer' [space] and then your valid JWT token.",
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
    });

    // Database context configuration
    services.AddDbContext<ArticleProjectContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DBConnection")));

    // Jwt token configuration
    var jwtSettings = builder.Configuration.GetSection("JWTSettings").Get<JWTSettings>()!;
    services.AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.ValidIssuer,
            ValidAudience = jwtSettings.ValidAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecurityKey))
        };
    });

    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireRole("Student", "Lecturer", "Admin")
        .Build();

    services.AddMvc(options =>
    {
        options.Filters.Add(new AuthorizeFilter(policy));
    });
}

void ConfigureMiddleware(WebApplication app)
{
    // Apply CORS policy here
    app.UseCors("CorsPolicy");

    app.UseSwagger();
    // Use Swagger in development environment
    if (app.Environment.IsDevelopment())
    {
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "ArticleProject API v1");
        });
    }

    // Middleware to redirect HTTP requests to HTTPS
    app.UseHttpsRedirection();

    // To use RESX, string localiser
    app.UseResources();

    // Use authentication and authorization
    app.UseAuthentication();
    app.UseAuthorization();

    // Add global Exception Handler
    app.UseExceptionHandler();

    // Map controllers and Identity API
    app.MapControllers();

    // Handle logging
    app.UseMiddleware<LoggingMiddleware>();

    // Handle error logging
    app.UseMiddleware<ErrorHandlingMiddleware>();
}

// Apply migrations automatically useful for DOCKER
void ApplyMigrations(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var dbContext = services.GetRequiredService<ArticleProjectContext>();
            dbContext.Database.Migrate();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while applying migrations: {ex.Message}");
        }
    }
}