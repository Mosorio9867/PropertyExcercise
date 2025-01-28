using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PropertyExercise.Services.DTO
{
    public class PropertyAddImageDto
    {
        [MaxLength(500)]
        public required string File { get; set; }
        [DefaultValue(true)]
        public required bool Enabled { get; set; }
    }
}
