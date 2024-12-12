using PhoneBookApi.Models;

namespace PhoneBookApi.Interfaces
{
    public interface IContactsValidation
    {
        void ValidateContact(Contact contact);
    }
}
