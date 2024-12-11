using System.ComponentModel.DataAnnotations;

namespace PhoneBookApi.DTOs
{
    public class ContactsDTO
    {
         public int Id { get; set; }

        // [Required]
        // [MaxLength(100)]
        public string Name { get; set; }

        // [Required]
        // [Phone]
        public string PhoneNumber { get; set; }

        // [EmailAddress]
        public string Email { get; set; }
    }
}