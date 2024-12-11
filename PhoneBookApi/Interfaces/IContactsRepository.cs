using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhoneBookApi.DTOs;
using PhoneBookApi.Models;

namespace PhoneBookApi.Interfaces
{
    public interface IContactsRepository
    { 
        void Create(Contact contact);
        Task<Contact> Put(int id, Contact contact);
        void Delete(Contact contact);
        Task<Contact> GetById(int id);
        Task<IEnumerable<Contact>> GetList();
        Task<bool> UpdateStatusAsync(int id);
        Task<bool> SaveAllAsync();
    }
}