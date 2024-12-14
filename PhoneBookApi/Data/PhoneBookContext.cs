using Microsoft.EntityFrameworkCore;
using PhoneBookApi.Models;

namespace PhoneBookApi.Data
{
    public class PhoneBookContext : DbContext
    {
        public PhoneBookContext(DbContextOptions<PhoneBookContext> options) : base(options)
        {

        }

        public virtual DbSet<Contact> Contacts { get; set; }
    }
}