using Microsoft.EntityFrameworkCore;
using TWMSServer.Model;

namespace TWMSServer.Helper
{
    public static class SeedDataAssetClass
    {
        // Optimized SeedAssetClassAsync method
        public static async Task SeedAssetClassAsync(TWMSServerContext context)
        {
            try
            {
                // Check if data already exists to avoid duplicate seeding
                if (await context.AssetClasses.AnyAsync())
                {
                    return;
                }

                // Read data from file
                var projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName;
                var filePath = Path.Combine(projectRoot, "Helper", "dataAssetClass.txt");

                var listAssetClass = ReadAssetClassData(filePath);

                if (listAssetClass.Any())
                {
                    // Add all asset classes in a single operation for better performance
                    await context.AssetClasses.AddRangeAsync(listAssetClass);
                    await context.SaveChangesAsync();

                    Console.WriteLine($"Successfully seeded {listAssetClass.Count} asset classes.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding asset classes: {ex.Message}");
                throw;
            }
        }
        // Method to read and parse the data file
        private static List<AssetClass> ReadAssetClassData(string filePath)
        {
            var assetClasses = new List<AssetClass>();
            var currentDateTime = DateTime.UtcNow;
            const string systemUser = "System";

            try
            {
                var lines = File.ReadAllLines(filePath);

                // Skip header line (first line)
                for (int i = 1; i < lines.Length; i++)
                {
                    var columns = lines[i].Split('\t');

                    if (columns.Length >= 3)
                    {
                        var assetClass = new AssetClass
                        {
                            AssetClassShortDesc = columns[1].Trim(),
                            AssetClassLongDesc = string.IsNullOrEmpty(columns[2].Trim()) ? columns[1].Trim() : columns[2].Trim(),
                            Status = true,
                            DateCreated = currentDateTime,
                            DateModified = currentDateTime,
                            CreatedBy = systemUser,
                            ModifiedBy = systemUser
                        };

                        assetClasses.Add(assetClass);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error reading asset class data: {ex.Message}", ex);
            }

            return assetClasses;
        }
    }
}
