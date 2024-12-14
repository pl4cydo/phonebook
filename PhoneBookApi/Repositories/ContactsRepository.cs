using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PhoneBookApi.Data;
using PhoneBookApi.Interfaces;
using PhoneBookApi.Models;

namespace PhoneBookApi.Repositories
{
    public class ContactsRepository : IContactsRepository
    {
        private readonly PhoneBookContext _context;

        public ContactsRepository(PhoneBookContext context)
        {
            _context = context;
        }

        public async Task<Contact> GetById(int id)
        {
            var contact = await _context.Contacts.Where(x => x.Id == id).FirstOrDefaultAsync();
            return contact!;
        }

        public async Task<Contact?> GetByPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number cannot be null or empty.", nameof(phoneNumber));

            phoneNumber = phoneNumber.Trim();

            return await _context.Contacts.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber && x.Status == 1);
        }

        public async Task<Contact?> GetByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));

            email = email.Trim();

            return await _context.Contacts.FirstOrDefaultAsync(x => x.Email == email && x.Status == 1);
        }

        public async Task<IEnumerable<Contact>> GetList()
        {
            return await _context.Contacts.ToListAsync();
        }

        public void Create(Contact newContact)
        {
            if (newContact == null)
            {
                throw new ArgumentNullException(nameof(newContact));
            }
            _context.Contacts.Add(newContact);
        }

        public void Delete(Contact contact)
        {
            _context.Contacts.Remove(contact);
        }

        public async Task<Contact> Put(int id, Contact contact)
        {
            Contact contactResult = await _context.Contacts.FindAsync(id);

            if (contactResult == null)
            {
                throw new KeyNotFoundException("Contact not found.");
            }

            contactResult.Name = contact.Name;
            contactResult.PhoneNumber = contact.PhoneNumber;
            contactResult.Email = contact.Email;

            await _context.SaveChangesAsync();

            return contactResult;
        }

        public async Task<bool> UpdateStatusAsync(int id)
        {
            Contact contactResult = _context.Contacts.Find(id)!;

            if (contactResult != null)
            {
                contactResult.Status = 0;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}