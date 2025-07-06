using ErpConnector.Repository.IRepository;
using ErpConnector.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;

namespace ErpConnector.Controllers
{
    public class TestController
    {
        private readonly ILogger<TestController> _logger;
        private readonly INopLocalizedPropertyRepository _nopNopLocalizedPropertyRepository;

        public TestController(ILogger<TestController> logger, INopLocalizedPropertyRepository nopNopLocalizedPropertyRepository)
        {
            _logger = logger;
            _nopNopLocalizedPropertyRepository = nopNopLocalizedPropertyRepository;
        }

        public async Task<bool> Main()
        {
            try
            {
                _logger.LogInformation("Starting LocalizedProperty test");
                Console.WriteLine("=========== LocalizedProperty Demo ===========");
                
                // 1. Show all existing records first
                Console.WriteLine("Current LocalizedProperty records:");
                await DisplayAllLocalizedProperties();
                
                // 2. Create dummy data for insert
                var dummyInsert = new LocalizedProperty
                {
                    LocaleKeyGroup = "Product",
                    LocaleKey = "Name",
                    LocaleValue = "Blue T-Shirt - German",
                    LanguageId = 2,  // Assuming 2 is German
                    EntityId = 101   // Assuming 101 is a product ID
                };
                
                // 3. Check if record exists
                var existingId = await _nopNopLocalizedPropertyRepository.GetLocalizedPropertyId(
                    dummyInsert.LocaleKeyGroup,
                    dummyInsert.LocaleKey,
                    dummyInsert.EntityId,
                    dummyInsert.LanguageId
                );
                
                int recordId;
                
                // 4. Insert or update based on existence
                if (existingId.HasValue)
                {
                    Console.WriteLine($"\nRecord already exists with ID {existingId}. Updating value...");
                    await _nopNopLocalizedPropertyRepository.UpdateLocalizedProperty(existingId.Value, dummyInsert.LocaleValue);
                    recordId = existingId.Value;
                    Console.WriteLine("Record updated successfully!");
                }
                else
                {
                    Console.WriteLine("\nInserting new localized property record...");
                    recordId = await _nopNopLocalizedPropertyRepository.InsertLocalizedProperty(dummyInsert);
                    Console.WriteLine($"New record inserted with ID: {recordId}");
                }
                
                // 5. Show records after insert/update
                Console.WriteLine("\nRecords after insert/update:");
                await DisplayAllLocalizedProperties();
                
                _logger.LogInformation("LocalizedProperty test completed successfully");
                Console.WriteLine("✅ Test completed successfully");
                return true;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error during test");
                Console.WriteLine("❌ Database error during test");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during test");
                Console.WriteLine($"❌ Unexpected error: {ex.Message}");
                return false;
            }
        }

        private async Task DisplayAllLocalizedProperties()
        {
            try
            {
                var localizedProperties = await _nopNopLocalizedPropertyRepository.GetAllLocalizedProperties();
                
                Console.WriteLine("\nLocalizedProperty Records:");
                Console.WriteLine("----------------------------------------");
                Console.WriteLine($"{"ID",-5} | {"KeyGroup",-15} | {"Key",-15} | {"Value",-30} | {"LangID",-5} | {"EntityID",-5}");
                Console.WriteLine("----------------------------------------");
                
                foreach (var prop in localizedProperties)
                {
                    Console.WriteLine($"{prop.Id,-5} | {prop.LocaleKeyGroup,-15} | {prop.LocaleKey,-15} | {(prop.LocaleValue?.Length > 30 ? prop.LocaleValue.Substring(0, 27) + "..." : prop.LocaleValue),-30} | {prop.LanguageId,-5} | {prop.EntityId,-5}");
                }
                
                Console.WriteLine("----------------------------------------");
                Console.WriteLine($"Total records: {localizedProperties.Count()}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error displaying localized properties");
                Console.WriteLine("❌ Error loading data for display");
            }
        }
    }
}
