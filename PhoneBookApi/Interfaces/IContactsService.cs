using PhoneBookApi.DTOs;
using PhoneBookApi.Models;

namespace PhoneBookApi.Interfaces
{
    public interface IContactsService
    {
        Task<bool> Create(Contact contact);
        Task<Contact> Put(int id, Contact contact);
        Task<bool> Delete(int id);
        Task<Contact> GetById(int id);
        Task<IEnumerable<Contact>> GetList();
    }
}