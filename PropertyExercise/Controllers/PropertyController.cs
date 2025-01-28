using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyExercise.Services.DTO;
using PropertyExercise.Services.Interfaces;

namespace PropertyExercise.Controllers
{
    // Authorizes access to only authenticated users
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _propertyService;

        // Constructor to inject the property service
        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        /// <summary>
        /// Creates a new property.
        /// </summary>
        /// <param name="propertyDto">DTO containing the details of the property to be created.</param>
        /// <returns>An IActionResult with a 200 OK status and the created property details.</returns>
        [HttpPost("CreateProperty")]
        public async Task<IActionResult> CreateProperty([FromBody] PropertyDto propertyDto)
        {
            // Calls the service to create the property
            PropertyDto property = await _propertyService.CreatePropertyAsync(propertyDto);
            return Ok(property); // Returns the created property
        }

        /// <summary>
        /// Adds an image to an existing property.
        /// </summary>
        /// <param name="idProperty">The ID of the property to which the image will be added.</param>
        /// <param name="propertyAddImageDto">DTO containing the image file and its status (enabled or not).</param>
        /// <returns>An IActionResult with a 204 No Content status if the image is successfully added.</returns>
        [HttpPost("{idProperty}/AddImageToProperty")]
        public async Task<IActionResult> AddImageToProperty(int idProperty, [FromBody] PropertyAddImageDto propertyAddImageDto)
        {
            // Calls the service to add the image to the property
            await _propertyService.AddImageToPropertyAsync(idProperty, propertyAddImageDto);
            return NoContent(); // Returns 204 No Content on success
        }

        /// <summary>
        /// Updates the price of an existing property.
        /// </summary>
        /// <param name="idProperty">The ID of the property whose price is to be updated.</param>
        /// <param name="newPrice">The new price to set for the property.</param>
        /// <returns>An IActionResult with a 204 No Content status if the price is successfully updated.</returns>
        [HttpPut("{idProperty}/ChangePrice")]
        public async Task<IActionResult> ChangePrice(int idProperty, [FromBody] decimal newPrice)
        {
            // Calls the service to change the property price
            await _propertyService.ChangePropertyPriceAsync(idProperty, newPrice);
            return NoContent(); // Returns 204 No Content on success
        }

        /// <summary>
        /// Updates the details of an existing property.
        /// </summary>
        /// <param name="idProperty">The ID of the property to be updated.</param>
        /// <param name="propertyDto">DTO containing the updated property details.</param>
        /// <returns>An IActionResult with a 204 No Content status if the property is successfully updated.</returns>
        [HttpPut("{idProperty}/UpdateProperty")]
        public async Task<IActionResult> UpdateProperty(int idProperty, [FromBody] PropertyDto propertyDto)
        {
            // Calls the service to update the property details
            await _propertyService.UpdatePropertyAsync(idProperty, propertyDto);
            return NoContent(); // Returns 204 No Content on success
        }

        /// <summary>
        /// Retrieves a list of properties that match the specified filters.
        /// </summary>
        /// <param name="filter">DTO containing the filter criteria (e.g., name, address, year, internal code).</param>
        /// <returns>An IActionResult with a 200 OK status and a list of properties that match the filter criteria.</returns>
        [HttpGet("GetProperties")]
        public async Task<IActionResult> GetProperties([FromQuery] PropertyFilterDto filter)
        {
            // Calls the service to get properties with the specified filters
            List<PropertyDto> listproperty = await _propertyService.GetPropertiesWithFiltersAsync(filter);
            return Ok(listproperty); // Returns the list of filtered properties
        }
    }
}
