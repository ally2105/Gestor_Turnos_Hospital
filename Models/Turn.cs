using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionDeTurnos.Web.Models
{
    public class Turn
    {
        public int Id { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? CalledAt { get; set; }

        public string? Status { get; set; } = "Pendiente";

        [ForeignKey("Box")]
        public int? BoxId { get; set; }
        public Box? Box { get; set; }
    }
}