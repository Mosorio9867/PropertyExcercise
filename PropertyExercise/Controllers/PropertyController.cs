using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyExercise.Services.DTO;
using PropertyExercise.Services.Interfaces;

namespace PropertyExercise.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _propertyService;

        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        [HttpPost("CreateProperty")]
        public async Task<IActionResult> CreateProperty([FromBody] PropertyDto propertyDto)
        {
            PropertyDto property = await _propertyService.CreatePropertyAsync(propertyDto);
            return Ok(property);
        }

        [HttpPost("{idProperty}/AddImageToProperty")]
        public async Task<IActionResult> AddImageToProperty(int idProperty, [FromBody] PropertyAddImageDto propertyAddImageDto)
        {
            await _propertyService.AddImageToPropertyAsync(idProperty, propertyAddImageDto);
            return NoContent();
        }
          
        [HttpPut("{idProperty}/ChangePrice")]
        public async Task<IActionResult> ChangePrice(int idProperty, [FromBody] decimal newPrice)
        {
            await _propertyService.ChangePropertyPriceAsync(idProperty, newPrice);
            return NoContent();
        }

        [HttpPut("{idProperty}/UpdateProperty")]
        public async Task<IActionResult> UpdateProperty(int idProperty, [FromBody] PropertyDto propertyDto)
        {
            await _propertyService.UpdatePropertyAsync(idProperty, propertyDto);
            return NoContent();
        }

        [HttpGet("GetProperties")]
        public async Task<IActionResult> GetProperties([FromQuery] PropertyFilterDto filter)
        {
            List<PropertyDto> listproperty = await _propertyService.GetPropertiesWithFiltersAsync(filter);
            return Ok(listproperty);
        }
    }
}
