using PhoneBookApi.Models;

namespace PhoneBookApi.Validation
{
    public interface IContactsValidation
    {
        void ValidateContact(Contact contact);
    }
}
