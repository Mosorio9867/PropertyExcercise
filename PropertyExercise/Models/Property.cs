using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertyExercise.Models
{
    [Table("Property")]
    public class Property
    {
        [Key]
        [Column(TypeName = "int")]
        [Display(Name = "Prénom")]
        public int IdProperty { get; set; } 
        public required string Name { get; set; } 
        public required string Address { get; set; } 
        public required decimal Price { get; set; } 
        public required string CodeInternal { get; set; }
        public required string Year { get; set; }

        [ForeignKey("OwnerEntity")]
        public required int IdOwner { get; set; }
        public virtual Owner? OwnerEntity { get; set; }
        public virtual ICollection<PropertyImage> PropertyImagesEntities { get; set; } = new List<PropertyImage>();
        public virtual ICollection<PropertyTrace> PropertyTraceEntities { get; set; } = new List<PropertyTrace>();

    }
}
