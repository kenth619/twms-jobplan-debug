
namespace TWMSServer.Helper
{
    public static class DbContextExtensions
    {
        public static async Task SeedDatabaseAsync(this TWMSServerContext context)
        {
            try
            {
                // Ensure database is created
                await context.Database.EnsureCreatedAsync();
                await SeedDataJobPlan.SeedJobPlanAsync(context);
                await SeedDataAssetClass.SeedAssetClassAsync(context);

                Console.WriteLine("Database seeded successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding database: {ex.Message}");
                throw;
            }
        }
    }
}
