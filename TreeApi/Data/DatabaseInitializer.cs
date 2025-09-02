using Microsoft.EntityFrameworkCore;

namespace TreeApi.Data
{
    public static class DatabaseInitializer
    {
        /// <summary>
        /// Initializes the database by applying migrations and seeding data if needed
        /// </summary>
        /// <param name="serviceProvider">The service provider to resolve dependencies</param>
        /// <returns>A task that represents the asynchronous initialization operation</returns>
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TreeApiDbContext>();
            
            try
            {
                if (await context.Database.GetPendingMigrationsAsync() is var pendingMigrations && 
                    pendingMigrations.Any())
                {
                    Console.WriteLine($"Applying {pendingMigrations.Count()} pending migrations...");
                    await context.Database.MigrateAsync();
                    Console.WriteLine("Migrations applied successfully.");
                }
                else
                {
                    Console.WriteLine("No pending migrations found.");
                }
                
                if (!await context.Database.CanConnectAsync())
                {
                    Console.WriteLine("Database does not exist. Creating...");
                    await context.Database.EnsureCreatedAsync();
                    Console.WriteLine("Database created successfully.");
                }
                
                await SeedDataIfNeededAsync(context);
                
                Console.WriteLine("Database initialization completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during database initialization: {ex.Message}");
                throw;
            }
        }
        
        /// <summary>
        /// Seeds the database with initial data if it's empty
        /// </summary>
        /// <param name="context">The database context</param>
        /// <returns>A task that represents the asynchronous seeding operation</returns>
        private static async Task SeedDataIfNeededAsync(TreeApiDbContext context)
        {
            if (!await context.Trees.AnyAsync())
            {
                Console.WriteLine("Seeding trees and nodes data...");
                await SeedData.SeedAsync(context);
                Console.WriteLine("Trees and nodes data seeded successfully.");
            }
            else
            {
                Console.WriteLine("Trees data already exists, skipping trees seeding.");
            }
            
            if (!await context.Partners.AnyAsync())
            {
                Console.WriteLine("Seeding partners data...");
                await SeedData.SeedPartnersAsync(context);
                Console.WriteLine("Partners data seeded successfully.");
            }
            else
            {
                Console.WriteLine("Partners data already exists, skipping partners seeding.");
            }
        }
    }
}
