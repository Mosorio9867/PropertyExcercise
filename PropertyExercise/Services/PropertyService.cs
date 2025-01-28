using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PropertyExercise.Context;
using PropertyExercise.Models;
using PropertyExercise.Services.DTO;
using PropertyExercise.Services.Interfaces;

namespace PropertyExercise.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly ApplicationDbContext _context;

        public PropertyService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new property in the database.
        /// </summary>
        /// <param name="propertyDto">The DTO object containing the property data to be created.</param>
        /// <returns>A <see cref="PropertyDto"/> object with the details of the created property.</returns>
        /// <exception cref="Exception">Throws an exception if a property with the same name already exists.</exception>
        public async Task<PropertyDto> CreatePropertyAsync(PropertyDto propertyDto)
        {
            try
            {
                // Check if a property with the same name already exists in the database.
                Property? propertyFind = _context.Properties
                    .FirstOrDefault(x => x.Name.Equals(propertyDto.Name));

                // If a property with the same name exists, throw an exception.
                if (propertyFind != null)
                    throw new Exception("A property already exists with the information entered, please verify.");

                // Retrieve the owner related to the property using the owner's ID.
                Owner owner = await GetOwner(propertyDto.IdOwner);

                Property property = new Property()
                {
                    Name = propertyDto.Name,
                    Address = propertyDto.Address,
                    Price = propertyDto.Price,
                    CodeInternal = propertyDto.CodeInternal,
                    Year = propertyDto.Year,
                    IdOwner = owner.IdOwner
                };
                // Add the new property to the database.
                _context.Properties.Add(property);

                // Save the changes made to the database.
                await _context.SaveChangesAsync();

                _context.PropertyTraces.Add(new PropertyTrace()
                {
                    IdProperty = property.IdProperty,
                    Name = propertyDto.Name,
                    DateSale = propertyDto.DateSale,
                    Value = propertyDto.Price,
                    Tax = propertyDto.Tax
                });

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // If an exception occurs, throw it with the error message.
                throw new Exception(ex.Message);
            }

            // Return the DTO of the created property.
            return propertyDto;
        }

        /// <summary>
        /// Adds an image to an existing property.
        /// </summary>
        /// <param name="idProperty">The ID of the property to which the image will be added.</param>
        /// <param name="propertyAddImageDto">DTO object that contains the image file and its enabled state.</param>
        /// <exception cref="Exception">Throws an exception if the property is not found or if an error occurs during the process.</exception>
        public async Task AddImageToPropertyAsync(int idProperty, PropertyAddImageDto propertyAddImageDto)
        {
            try
            {
                // Retrieve the existing property using its ID.
                Property property = await GetProperty(idProperty);

                // Create a new image object and associate it with the property.
                _context.PropertyImages.Add(new PropertyImage
                {
                    IdProperty = property.IdProperty,
                    File = propertyAddImageDto.File,
                    Enabled = propertyAddImageDto.Enabled
                });

                // Save the changes made to the database.
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // If an exception occurs, throw it with the error message.
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Changes the price of an existing property.
        /// </summary>
        /// <param name="idProperty">The ID of the property whose price needs to be updated.</param>
        /// <param name="newPrice">The new price of the property.</param>
        /// <exception cref="Exception">Throws an exception if the property is not found or if an error occurs during the process.</exception>
        public async Task ChangePropertyPriceAsync(int idProperty, decimal newPrice)
        {
            try
            {
                // Retrieve the existing property using its ID.
                Property property = await GetProperty(idProperty);

                // Change the price of the property to the new value.
                property.Price = newPrice;

                // Update the property in the database.
                _context.Properties.Update(property);

                // Save the changes made to the database.
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // If an exception occurs, throw it with the error message.
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates the details of an existing property.
        /// </summary>
        /// <param name="idProperty">The ID of the property to update.</param>
        /// <param name="propertyDto">DTO object containing the new data for the property.</param>
        /// <exception cref="Exception">Throws an exception if the property is not found or if an error occurs during the process.</exception>
        public async Task UpdatePropertyAsync(int idProperty, PropertyDto propertyDto)
        {
            try
            {
                // Retrieve the existing property using its ID.
                Property property = await GetProperty(idProperty);

                // Retrieve the owner associated with the property using the owner's ID.
                Owner owner = await GetOwner(propertyDto.IdOwner);

                // Update the property details with the new values provided.
                property.Name = propertyDto.Name;
                property.Address = propertyDto.Address;
                property.Price = propertyDto.Price;
                property.CodeInternal = propertyDto.CodeInternal;
                property.Year = propertyDto.Year;
                property.IdOwner = owner.IdOwner;

                // Update the property in the database.
                _context.Properties.Update(property);

                _context.PropertyTraces.Add(new PropertyTrace()
                {
                    IdProperty = property.IdProperty,
                    Name = propertyDto.Name,
                    DateSale = propertyDto.DateSale,
                    Value = propertyDto.Price,
                    Tax = propertyDto.Tax
                });

                // Save the changes made to the database.
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // If an exception occurs, throw it with the error message.
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a list of properties that match the specified filters.
        /// </summary>
        /// <param name="filter">DTO object containing the search filters (name, address, internal code, year).</param>
        /// <returns>A list of <see cref="PropertyDto"/> objects that meet the applied filters.</returns>
        public async Task<List<PropertyDto>> GetPropertiesWithFiltersAsync(PropertyFilterDto filter)
        {
            // Perform a query to the database applying the provided filters.
            List<PropertyDto> listProperty = await _context.Properties
                .AsQueryable()
                .Where(x => (filter.Name.IsNullOrEmpty() || x.Name.Contains(filter.Name))
                         && (filter.Address.IsNullOrEmpty() || x.Address.Contains(filter.Address))
                         && (filter.CodeInternal.IsNullOrEmpty() || x.CodeInternal.Contains(filter.CodeInternal))
                         && (filter.Year.IsNullOrEmpty() || x.Year.Contains(filter.Year)))
                .Select(x => new PropertyDto()
                {
                    IdProperty = x.IdProperty,
                    Name = x.Name,
                    Address = x.Address,
                    Price = x.Price,
                    CodeInternal = x.CodeInternal,
                    Year = x.Year,
                    IdOwner = x.IdOwner,
                    DateSale = DateTime.Now,
                    Tax = 0
                })
                .ToListAsync();

            // Return the list of filtered properties.
            return listProperty;
        }

        /// <summary>
        /// Retrieves a property by its ID.
        /// </summary>
        /// <param name="idProperty">The ID of the property to retrieve.</param>
        /// <returns>The property corresponding to the provided ID.</returns>
        /// <exception cref="Exception">Throws an exception if the property is not found.</exception>
        private async Task<Property> GetProperty(int idProperty)
        {
            // Search for the property in the database using the provided ID.
            Property? property = await _context.Properties.FindAsync(idProperty);

            // If the property is not found, throw an exception.
            if (property == null)
                throw new Exception("No information was found for the property entered, please verify.");

            // Return the found property.
            return property;
        }

        /// <summary>
        /// Retrieves an owner by their ID.
        /// </summary>
        /// <param name="idOwner">The ID of the owner to retrieve.</param>
        /// <returns>The owner corresponding to the provided ID.</returns>
        /// <exception cref="Exception">Throws an exception if the owner is not found.</exception>
        private async Task<Owner> GetOwner(int idOwner)
        {
            // Search for the owner in the database using the provided ID.
            Owner? property = await _context.Owners.FindAsync(idOwner);

            // If the owner is not found, throw an exception.
            if (property == null)
                throw new Exception("No information was found for the owner entered, please verify.");

            // Return the found owner.
            return property;
        }
    }
}
