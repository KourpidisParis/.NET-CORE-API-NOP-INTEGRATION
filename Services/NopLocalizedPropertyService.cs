using ErpConnector.DTOs;
using ErpConnector.Models;
using ErpConnector.Mappers.IMappers;
using ErpConnector.Repository.IRepository;
using ErpConnector.Services.IServices;

namespace ErpConnector.Services
{
    public class NopLocalizedPropertyService : INopLocalizedPropertyService
    {
        private readonly INopLocalizedPropertyRepository _nopNopLocalizedPropertyRepository;

        public NopLocalizedPropertyService(INopLocalizedPropertyRepository nopNopLocalizedPropertyRepository)
        {
            _nopNopLocalizedPropertyRepository = nopNopLocalizedPropertyRepository;
        }

        public async Task HandleLocalizedProperty(LocalizedProperty localizedProperty)
        {            
            var existingId = await _nopNopLocalizedPropertyRepository.GetLocalizedPropertyId(
                localizedProperty.LocaleKeyGroup,
                localizedProperty.LocaleKey,
                localizedProperty.EntityId,
                localizedProperty.LanguageId
            );
            
            int recordId;
            
            //Insert or update based on existence
            if (existingId.HasValue)
            {
                Console.WriteLine($"\nRecord already exists with ID {existingId}. Updating value...");
                await _nopNopLocalizedPropertyRepository.UpdateLocalizedProperty(existingId.Value, localizedProperty.LocaleValue);
                recordId = existingId.Value;
                Console.WriteLine("Record updated successfully!");
            }
            else
            {
                Console.WriteLine("\nInserting new localized property record...");
                recordId = await _nopNopLocalizedPropertyRepository.InsertLocalizedProperty(localizedProperty);
                Console.WriteLine($"New record inserted with ID: {recordId}");
            }
        }
    }
}
