
using Microsoft.AspNetCore.Http.HttpResults;
using PhoneBookApi.Interfaces;
using PhoneBookApi.Models;
using PhoneBookApi.Validation;

namespace PhoneBookApi.Services
{
    public class ContactsService : IContactsService
    {
        private readonly IContactsRepository _contactsRepository;
        private readonly IContactsValidation _contactsValidation;

        public ContactsService(IContactsRepository contactsRepository, IContactsValidation contactsValidation)
        {
            _contactsRepository = contactsRepository;
            _contactsValidation = contactsValidation;
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
            return contactList.Where(contact => contact.Status == 1).OrderBy(contact => contact.Name);
        }
        public async Task<bool> Create(Contact newContact)
        {
            _contactsValidation.ValidateContact(newContact);

            var contacCheckPhoneNumber = await _contactsRepository.GetByPhoneNumber(newContact.PhoneNumber);

            if (contacCheckPhoneNumber != null)
            {
                throw new InvalidOperationException($"Phone Number alredy exists");
            }

            var contacCheckEmail = await _contactsRepository.GetByEmail(newContact.Email);

            if (contacCheckEmail != null)
            {
                throw new InvalidOperationException($"Email alredy exists");
            }

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
            _contactsValidation.ValidateContact(contact);
            
            var contactResult = await _contactsRepository.GetById(id);

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