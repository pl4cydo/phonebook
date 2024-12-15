using System.ComponentModel.DataAnnotations;

namespace PhoneBookApi.DTOs
{
    public class ContactsDTO
    {
         public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}