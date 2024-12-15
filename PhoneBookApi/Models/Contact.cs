using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PhoneBookApi.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(14)]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [DefaultValue(1)]
        public int Status { get; set; } = 1;
    }
}