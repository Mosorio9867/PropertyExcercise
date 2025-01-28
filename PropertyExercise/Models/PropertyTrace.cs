using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertyExercise.Models
{
    public class PropertyTrace
    {
        [Key]
        public int IdPropertyTrace { get; set; }

        [ForeignKey("PropertyEntity")]
        public required int IdProperty { get; set; }
        public required DateTime DateSale { get; set; } 
        public required string Name { get; set; } 
        public required decimal Value { get; set; } 
        public required decimal Tax { get; set; } 
        public required virtual Property PropertyEntity { get; set; }
    }
}
