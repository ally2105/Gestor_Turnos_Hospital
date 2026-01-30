using System.ComponentModel.DataAnnotations;

namespace GestionDeTurnos.Web.Models
{
    public class Box
    {
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        public bool IsOpen { get; set; } = true;

        public string? AssignedUser { get; set; }
    }
}