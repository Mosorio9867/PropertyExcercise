using System.ComponentModel.DataAnnotations;

namespace PropertyExercise.Models
{
    public class Owner
    {
        [Key]
        public int IdOwner { get; set; } 
        public required string Name { get; set; } 
        public required string Address { get; set; } 
        public required string Photo { get; set; } 
        public required DateTime Birthday { get; set; }
    }
}
