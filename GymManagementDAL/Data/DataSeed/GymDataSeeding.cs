using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.DataSeed
{
    public static class GymDataSeeding
    {
        public static async Task<bool> SeedData(GymDbContext context)
        {
            try
            {
                if (!context.Categories.Any()) // Check if context contains any data
                {
                    var categories = await LoadDataFromJsonFileAsync<Category>("categories.json"); // // Load category data asynchronously from JSON file
                    context.Categories.AddRange(categories); // Add data to categories
                }

                if (!context.Plans.Any())
                {
                    var plans = await LoadDataFromJsonFileAsync<Plan>("plans.json");
                    context.Plans.AddRange(plans);
                }

                return await context.SaveChangesAsync() > 0; 
            }
            catch (Exception ex) 
            {
                return false;
            }
        }


        #region MyRegion

        // This method asynchronously loads a JSON file from wwwroot/files,
        // checks if it exists, reads its content, and deserializes it into a List<T>.
        private static async Task<List<T>> LoadDataFromJsonFileAsync<T>(string fileName)
        {
            try
            {
                // Build the full path to the file inside wwwroot/files
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", fileName);

                // Check if file exists before reading
                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"File not found: {filePath}");

                // Read JSON file content asynchronously
                var jsonData = await File.ReadAllTextAsync(filePath);

                // Setup JSON options (case-insensitive + enum support)
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                options.Converters.Add(new JsonStringEnumConverter());

                // Deserialize JSON into List<T> and return it
                var result = JsonSerializer.Deserialize<List<T>>(jsonData, options);

                // Return empty list if JSON is null (for safety)
                return result ?? new List<T>();
            }
            catch (Exception ex)
            {
                // Log or handle the error here if needed
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return new List<T>(); // Return empty list instead of crashing
            }
        }
        #endregion
    }
}
