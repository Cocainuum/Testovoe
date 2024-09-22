using Microsoft.EntityFrameworkCore;
using WebService.Configuration;
using WebService.Middlewares;
using WebService.Persistence.Contexts;
using WebService.Repositories;
using WebService.Services;

namespace WebService;

public static class Startup
{
    public static void AddServices(this IServiceCollection services, 
        ConfigurationManager configuration)
    {
        var databaseConfiguration = configuration.GetSection("Database").Get<DatabaseConfiguration>();

        services.AddDbContext<DatabaseContext>(opt =>
        {
            opt.UseSqlite(databaseConfiguration.ConnectionString);
        });
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddControllers();
        services.AddCors();

        services.AddScoped<IMedicineRepository, MedicineRepository>();
        services.AddScoped<IMedicineService, MedicineService>();
    }

    public static void Configure(this WebApplication app)
    {
        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.UseRouting();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        
        app.UseSwagger();
        app.UseSwaggerUI();
        
        var logger = app.Logger;
        
        var migrateDb = app.Configuration.GetSection("Database").Get<DatabaseConfiguration>()?.MigrateDB;

        if (migrateDb.GetValueOrDefault(false))
        {
            logger.LogWarning("Automatic migration is enabled.");
            
            using var scope = app.Services.CreateScope();
            logger.LogInformation("Trying to migrate database");
            
            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            var migrations = dbContext.Database.GetPendingMigrations().ToList();
            logger.LogInformation(
                $"{migrations.Count} migrations will be applied: {string.Join(", ", migrations)}");
            
            dbContext.Database.Migrate();
            logger.LogInformation("All migrations are applied to database");
        }
    }
}