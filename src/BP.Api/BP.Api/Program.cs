using BP.Api.ExtensionMethods;
using BP.Api.Options;
using BP.Application.Interfaces.Options;
using BP.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using System.Threading.RateLimiting;

namespace BP.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //builder.WebHost.ConfigureKestrel(options =>
        //{
        //    options.ListenAnyIP(5000); // binds to 0.0.0.0:5000
        //});

        //builder.WebHost.UseUrls("http://0.0.0.0:5000");
        builder.AddServiceDefaults();
        builder.Services.AddOptions<JwtOptions>()
            .Bind(builder.Configuration.GetSection(nameof(JwtOptions)))
            .Validate(o => !string.IsNullOrWhiteSpace(o.Key) && o.Key.Length >= 32, $"JwtOptions:Key must be at least 32 characters.")
            .Validate(o => !string.IsNullOrWhiteSpace(o.Issuer), "JwtOptions:Issuer is required.")
            .Validate(o => o.Audience is { Count: > 0 }, "JwtOptions:Audience must include at least one value.")
            .ValidateOnStart();

        System.Diagnostics.Debug.WriteLine("JwtOptions configured with Key length: " + builder.Configuration.GetSection(nameof(JwtOptions)).GetValue<string>("Key")?.Length);
        builder.Services.Configure<UserTableSeedingOptions>(builder.Configuration.GetSection(nameof(UserTableSeedingOptions)));

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddApplicationServices();
        builder.Services.AddAzureTableServices();
        builder.Services.AddAzureBlobServices();
        builder.Services.AddRepositories();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();
        builder.Services.AddAuthorization(); // No fallback policy

        builder.Services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.AddFixedWindowLimiter("login", rateOptions =>
            {
                rateOptions.PermitLimit = 5;
                rateOptions.Window = TimeSpan.FromMinutes(1);
                rateOptions.QueueLimit = 0;
                rateOptions.AutoReplenishment = true;
            });
        });

        builder.Services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();

        // Add CORS policies
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });

            options.AddPolicy("AllowSpecificOrigins", policy =>
            {
                policy.WithOrigins(
                    "https://your-production-origin.com",
                    "https://another-production-origin.com"
                )
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
        });

        builder.Services.AddHealthChecks();

        var app = builder.Build();

        //app.MapDefaultEndpoints();
        app.MapHealthChecks("/health").AllowAnonymous();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {

            using var scope = app.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredKeyedService<ISeederService>("User");
            await seeder.SeedAsync();


            app.MapOpenApi();
            // Serve OpenAPI JSON at /swagger/v1/swagger.json
            app.UseSwagger();

            // Serve Swagger UI at /swagger
            app.UseSwaggerUI();

        }

        app.UseHttpsRedirection();

        // Use CORS policy based on environment
        if (app.Environment.IsDevelopment())
        {
            app.UseCors("AllowAll");
        }
        else
        {
            app.UseCors("AllowSpecificOrigins");
        }

        app.UseRateLimiter();
        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
