using System;
using System.ComponentModel.DataAnnotations;

namespace GestionDeTurnos.Web.Models
{
    public class Affiliate
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public required string FirstName { get; set; }

        [Required, StringLength(100)]
        public required string LastName { get; set; }

        [Required, StringLength(20)]
        public required string DocumentNumber { get; set; }

        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? PhotoPath { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}