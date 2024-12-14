using PhoneBookApi.Models;

namespace PhoneBookApi.Interfaces
{
    public interface IContactsRepository
    { 
        void Create(Contact contact);
        Task<Contact> Put(int id, Contact contact);
        void Delete(Contact contact);
        Task<Contact> GetById(int id);
        Task<Contact?> GetByPhoneNumber(string phoneNumber);
        Task<Contact?> GetByEmail(string email);
        Task<IEnumerable<Contact>> GetList();
        Task<bool> UpdateStatusAsync(int id);
        Task<bool> SaveAllAsync();
    }
}