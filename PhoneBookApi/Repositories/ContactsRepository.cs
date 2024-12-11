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
        private readonly IMapper _mapper;

        public ContactsRepository(PhoneBookContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Contact> GetById(int id)
        {
            var contact = await _context.Contacts.Where(x => x.Id == id).FirstOrDefaultAsync();
            return contact!;
        }

        public async Task<Contact> GetByPhoneNumber(string phoneNumber)
        {
            var contact = await _context.Contacts.Where(x => x.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
            return contact!;
        }

        public async Task<Contact> GetByEmail(string email)
        {
            var contact = await _context.Contacts.Where(x => x.Email == email).FirstOrDefaultAsync();
            return contact!;
        }

        public async Task<IEnumerable<Contact>> GetList()
        {
            return await _context.Contacts.ToListAsync();
        }

        public void Create(Contact newContact)
        {   
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