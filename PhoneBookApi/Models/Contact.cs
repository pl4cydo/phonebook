using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PhoneBookApi.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [Phone]
        [StringLength(13)]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [DefaultValue(1)]
        public int Status { get; set; } = 1;
    }
}