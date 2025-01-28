using PropertyExercise.Services.DTO;

namespace PropertyExercise.Services.Interfaces
{
    public interface IPropertyService
    {
        /// <summary>
        /// Asynchronously creates a new property using the provided property data.
        /// </summary>
        /// <param name="propertyDto">The DTO object containing the property data to be created.</param>
        /// <returns>A <see cref="PropertyDto"/> object with the details of the created property.</returns>
        Task<PropertyDto> CreatePropertyAsync(PropertyDto propertyDto);

        /// <summary>
        /// Asynchronously adds an image to an existing property.
        /// </summary>
        /// <param name="idProperty">The ID of the property to which the image will be added.</param>
        /// <param name="propertyAddImageDto">The DTO object containing the image and its enabled state.</param>
        Task AddImageToPropertyAsync(int idProperty, PropertyAddImageDto propertyAddImageDto);

        /// <summary>
        /// Asynchronously updates the price of an existing property.
        /// </summary>
        /// <param name="idProperty">The ID of the property whose price will be updated.</param>
        /// <param name="newPrice">The new price to set for the property.</param>
        Task ChangePropertyPriceAsync(int idProperty, decimal newPrice);

        /// <summary>
        /// Asynchronously updates the details of an existing property.
        /// </summary>
        /// <param name="idProperty">The ID of the property to be updated.</param>
        /// <param name="propertyDto">The DTO object containing the updated property details.</param>
        Task UpdatePropertyAsync(int idProperty, PropertyDto propertyDto);

        /// <summary>
        /// Asynchronously retrieves a list of properties that match the provided filters.
        /// </summary>
        /// <param name="filter">The DTO object containing the filter criteria (e.g., name, address, internal code, year).</param>
        /// <returns>A list of <see cref="PropertyDto"/> objects that match the provided filters.</returns>
        Task<List<PropertyDto>> GetPropertiesWithFiltersAsync(PropertyFilterDto filter);
    }
}
