using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertyExercise.Models
{
    public class PropertyImage
    {
        [Key]
        public int IdPropertyImage { get; set; }

        [ForeignKey("PropertyEntity")]
        public required int IdProperty { get; set; }
        public required string File { get; set; } 
        public required bool Enabled { get; set; } 
        public virtual Property? PropertyEntity { get; set; }
    }
}
