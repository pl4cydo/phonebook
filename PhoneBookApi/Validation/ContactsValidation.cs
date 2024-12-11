using FluentValidation;
using FluentValidation.Results;
using PhoneBookApi.Models;

namespace PhoneBookApi.Validation
{
    public class ContactsValidation : AbstractValidator<Contact>, IContactsValidation
    {
        public ContactsValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MaximumLength(100).WithMessage("O nome não pode ter mais que 50 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O e-mail é obrigatório.")
                .EmailAddress().WithMessage("O e-mail deve ser válido.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("O número de telefone é obrigatório.")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("O número de telefone deve ser um número válido no formato internacional.");
        }

        public void ValidateContact(Contact contact)
        {
            var validationResult = Validate(contact);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }
    }
}
