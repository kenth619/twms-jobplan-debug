using Microsoft.EntityFrameworkCore;
using TWMSServer.Model;

namespace TWMSServer.Helper
{
    public static class SeedDataJobPlan
    {
        // Optimized SeedAssetClassAsync method
        public static async Task SeedJobPlanAsync(TWMSServerContext context)
        {
            try
            {
                // Check if data already exists to avoid duplicate seeding
                if (await context.JobPlansHeaders.AnyAsync())
                {
                    return;
                }

                // Read data from file
                var projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName;
                var filePath = Path.Combine(projectRoot, "Helper", "dataJobPlansHeader.txt");

                var listJobPlansHeaders = ReadAssetClassData(filePath);

                if (listJobPlansHeaders.Any())
                {
                    // Add all job plan in a single operation for better performance
                    await context.JobPlansHeaders.AddRangeAsync(listJobPlansHeaders);
                    await context.SaveChangesAsync();

                    Console.WriteLine($"Successfully seeded {listJobPlansHeaders.Count} job plan.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding job plan: {ex.Message}");
                throw;
            }
        }
        // Method to read and parse the data file
        private static List<JobPlansHeader> ReadAssetClassData(string filePath)
        {
            var listJobPlansHeader = new List<JobPlansHeader>();
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
                        var assetClass = new JobPlansHeader
                        {
                            JobPlanStatus = true,
                            JobPlanShortDesc = columns[1].Trim(),
                            JobPlanLongDesc = string.IsNullOrEmpty(columns[2].Trim()) ? columns[1].Trim() : columns[2].Trim(),
                            AssetClassShortDesc = "",
                            DateCreated = currentDateTime,
                            DateModified = currentDateTime,
                            CreatedBy = systemUser,
                            ModifiedBy = systemUser
                        };

                        listJobPlansHeader.Add(assetClass);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error reading asset class data: {ex.Message}", ex);
            }

            return listJobPlansHeader;
        }
    }
}
