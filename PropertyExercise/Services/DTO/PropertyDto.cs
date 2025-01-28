using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PropertyExercise.Services.DTO
{
    public class PropertyDto
    {
        public int? IdProperty { get; set; }
        [MaxLength(100)]
        public required string Name { get; set; }
        [MaxLength(250)]
        public required string Address { get; set; }
        public required decimal Price { get; set; }
        [MaxLength(10)]
        public required string CodeInternal { get; set; }
        [MaxLength(4)]
        public required string Year { get; set; }
        [DefaultValue(1)]
        public required int IdOwner { get; set; }
    }
}
