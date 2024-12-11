
using PhoneBookApi.Interfaces;
using PhoneBookApi.Models;

namespace PhoneBookApi.Services
{
    public class ContactsService : IContactsService
    {
        private readonly IContactsRepository _contactsRepository;

        public ContactsService(IContactsRepository contactsRepository)
        {
            _contactsRepository = contactsRepository;
        }

        public async Task<Contact> GetById(int id)
        {
            Contact contact = await _contactsRepository.GetById(id);

            if (contact == null)
            {
                throw new KeyNotFoundException($"Contact with ID {id} not found.");
            }

            if (contact.Status == 0)
            {
                throw new InvalidOperationException("Contact is inactive.");
            }

            return contact;
        }

        public async Task<IEnumerable<Contact>> GetList()
        {
            IEnumerable<Contact> contactList = await _contactsRepository.GetList();
            return contactList.Where(contact => contact.Status == 1);
        }
        public async Task<bool> Create(Contact newContact)
        {
            _contactsRepository.Create(newContact);
            return await _contactsRepository.SaveAllAsync();
        }

        public async Task<bool> Delete(int id)
        {
            Contact contactResult = await _contactsRepository.GetById(id);

            if (contactResult == null)
            {
                throw new KeyNotFoundException($"Contact with ID {id} not found.");
            }

            if (contactResult.Status == 0)
            {
                throw new InvalidOperationException("Contact is inactive.");
            }

            return await _contactsRepository.UpdateStatusAsync(id);
        }

        public async Task<Contact> Put(int id, Contact contact)
        {
            Contact contactResult = await _contactsRepository.GetById(id);

            if (contactResult == null)
            {
                throw new KeyNotFoundException($"Contact with ID {id} not found.");
            }

            if (contactResult.Status == 0)
            {
                throw new InvalidOperationException("Contact is inactive.");
            }
        
            return await _contactsRepository.Put(id, contact);
        }
    }
}