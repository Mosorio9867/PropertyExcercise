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

        public async Task<PropertyDto> CreatePropertyAsync(PropertyDto propertyDto)
        {
            try
            {
                Property? property = _context.Properties
                    .FirstOrDefault(x => x.Name.Equals(propertyDto.Name));

                if (property != null)
                    throw new Exception("A property already exists with the information entered, please verify.");

                Owner owner = await GetOwner(propertyDto.IdOwner);
                _context.Properties.Add(new Property()
                {
                    Name = propertyDto.Name,
                    Address = propertyDto.Address,
                    Price = propertyDto.Price,
                    CodeInternal = propertyDto.CodeInternal,
                    Year = propertyDto.Year,
                    IdOwner = owner.IdOwner,
                });

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return propertyDto;
        }

        public async Task AddImageToPropertyAsync(int idProperty, PropertyAddImageDto propertyAddImageDto)
        {
            try
            {
                Property property = await GetProperty(idProperty);

                _context.PropertyImages.Add(new PropertyImage { 
                    IdProperty = property.IdProperty, 
                    File = propertyAddImageDto.File, 
                    Enabled = propertyAddImageDto.Enabled 
                });

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task ChangePropertyPriceAsync(int idProperty, decimal newPrice)
        {
            try
            {
                Property property = await GetProperty(idProperty);

                property.Price = newPrice;
                _context.Properties.Update(property);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdatePropertyAsync(int idProperty, PropertyDto propertyDto)
        {
            try
            {
                Property property = await GetProperty(idProperty);
                Owner owner = await GetOwner(propertyDto.IdOwner);

                property.Name = propertyDto.Name;
                property.Address = propertyDto.Address;
                property.Price = propertyDto.Price;
                property.CodeInternal = propertyDto.CodeInternal;
                property.Year = propertyDto.Year;
                property.IdOwner = owner.IdOwner;
                _context.Properties.Update(property);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<PropertyDto>> GetPropertiesWithFiltersAsync(PropertyFilterDto filter)
        {
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
                    IdOwner = x.IdOwner
                })
                .ToListAsync();

            return listProperty;
        }

        private async Task<Property> GetProperty(int idProperty)
        {
            Property? property = await _context.Properties.FindAsync(idProperty);

            if (property == null)
                throw new Exception("No information was found for the property entered, please verify.");

            return property;
        }

        private async Task<Owner> GetOwner(int idOwner)
        {
            Owner? property = await _context.Owners.FindAsync(idOwner);

            if (property == null)
                throw new Exception("No information was found for the owner entered, please verify.");

            return property;
        }
    }
}
