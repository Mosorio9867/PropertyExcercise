using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PropertyExercise.Services.DTO;

namespace PropertyExercise.Services.Interfaces
{
    public interface IPropertyService
    {
        Task<PropertyDto> CreatePropertyAsync(PropertyDto propertyDto);
        Task AddImageToPropertyAsync(int idProperty, PropertyAddImageDto propertyAddImageDto);
        Task ChangePropertyPriceAsync(int idProperty, decimal newPrice);
        Task UpdatePropertyAsync(int idProperty, PropertyDto propertyDto);
        Task<List<PropertyDto>> GetPropertiesWithFiltersAsync(PropertyFilterDto filter);
    }
}
