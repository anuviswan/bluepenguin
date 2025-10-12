using BP.Api.ExtensionMethods;
using BP.Api.Options;
using BP.Application.Interfaces.Options;
using BP.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

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
        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
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

        var app = builder.Build();

        app.MapDefaultEndpoints();

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

        //app.UseHttpsRedirection();

        // Use CORS policy based on environment
        if (app.Environment.IsDevelopment())
        {
            app.UseCors("AllowAll");
        }
        else
        {
            app.UseCors("AllowSpecificOrigins");
        }

        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
