using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PhoneBookApi.Services;
using PhoneBookApi.Repositories;
using PhoneBookApi.Models;
using PhoneBookApi.Interfaces;


namespace PhoneBookTests.Tests.Service
{
    public class ContactsServiceTests
    {
        private readonly Mock<IContactsRepository> _contactsRepositoryMock;
        private readonly Mock<IContactsValidation> _contactsValidationMock;
        private readonly ContactsService _service;

        public ContactsServiceTests()
        {
            _contactsRepositoryMock = new Mock<IContactsRepository>();
            _contactsValidationMock = new Mock<IContactsValidation>();
            _service = new ContactsService(_contactsRepositoryMock.Object, _contactsValidationMock.Object);
        }

        [Fact]
        public async Task GetById_ShouldReturnContact_WhenContactExists()
        {
            // Arrange
            int contactId = 1;

            // Mock do objeto retornado pelo repositorio
            var contact = new Contact
            {
                Id = contactId,
                Name = "Test Contact",
                PhoneNumber = "5581986482982",
                Email = "placydo@gmail.com",
                Status = 1
            };

            //Mock do repositorio
            _contactsRepositoryMock.Setup(r => r.GetById(contactId)).ReturnsAsync(contact);

            // Act
            var result = await _service.GetById(contactId);

            // Acert
            Assert.Equal(contactId, result.Id);
        }

        [Fact]
        public async Task GetById_ShouldThrowAException_WhenContactIsNull()
        {
            // Arrange
            int contactId = 500;

            // Mock do objeto retornado pelo repositório
            Contact contact = null;

            // Mock do repositório
            _contactsRepositoryMock.Setup(r => r.GetById(contactId)).ReturnsAsync(contact);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetById(contactId));

            // Verifica se a mensagem de exceção é a esperada
            Assert.Equal($"Contact with ID {contactId} not found.", exception.Message);
        }

        [Fact]
        public async Task GetById_ShouldThrowAException_WhenContactStatusIsZero()
        {
            // Arrange
            int contactId = 500;

            // Mock do objeto retornado pelo repositório
            var contact = new Contact
            {
                Id = contactId,
                Name = "Test Contact",
                PhoneNumber = "5581986482982",
                Email = "placydo@gmail.com",
                Status = 0
            };

            // Mock do repositório
            _contactsRepositoryMock.Setup(r => r.GetById(contactId)).ReturnsAsync(contact);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.GetById(contactId));

            // Verifica se a mensagem de exceção é a esperada
            Assert.Equal("Contact is inactive.", exception.Message);
        }

        [Fact]
        public async Task GetList_ShouldReturnOnlyActiveContacts()
        {
            // Arrange
            var contacts = new List<Contact>
            {
                new Contact { Id = 1, Name = "Active Contact", PhoneNumber = "1234567890", Email = "active@example.com", Status = 1 },
                new Contact { Id = 2, Name = "Inactive Contact", PhoneNumber = "0987654321", Email = "inactive@example.com", Status = 0 },
                new Contact { Id = 3, Name = "Another Active Contact", PhoneNumber = "5555555555", Email = "anotheractive@example.com", Status = 1 }
            };

            // Mock do repositório
            _contactsRepositoryMock.Setup(r => r.GetList()).ReturnsAsync(contacts);

            // Act
            var result = await _service.GetList();

            // Assert
            var activeContacts = result.ToList();
            Assert.Equal(2, activeContacts.Count); // Verifica se o número de contatos ativos é 2
            Assert.All(activeContacts, contact => Assert.Equal(1, contact.Status)); // Verifica se todos os contatos têm Status == 1
        }

        [Fact]
        public async Task Create_ShouldReturnTrue_WhenContactIsValidAndDoesNotExist()
        {
            // Arrange
            var newContact = new Contact
            {
                Name = "New Contact",
                PhoneNumber = "1234567890",
                Email = "newcontact@example.com",
                Status = 1
            };

            // Mock das validações
            _contactsValidationMock.Setup(v => v.ValidateContact(newContact));

            // Mock do repositório para retornar null para telefone e e-mail
            _contactsRepositoryMock.Setup(r => r.GetByPhoneNumber(newContact.PhoneNumber)).ReturnsAsync((Contact)null);
            _contactsRepositoryMock.Setup(r => r.GetByEmail(newContact.Email)).ReturnsAsync((Contact)null);

            // Mock do método Create
            _contactsRepositoryMock.Setup(r => r.Create(newContact));

            // Mock do SaveAllAsync para retornar true
            _contactsRepositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);

            // Act
            var result = await _service.Create(newContact);

            // Assert
            Assert.True(result);
            _contactsValidationMock.Verify(v => v.ValidateContact(newContact), Times.Once);
            _contactsRepositoryMock.Verify(r => r.GetByPhoneNumber(newContact.PhoneNumber), Times.Once);
            _contactsRepositoryMock.Verify(r => r.GetByEmail(newContact.Email), Times.Once);
            _contactsRepositoryMock.Verify(r => r.Create(newContact), Times.Once);
            _contactsRepositoryMock.Verify(r => r.SaveAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Create_ShouldThrowInvalidOperationException_WhenPhoneNumberAlreadyExists()
        {
            // Arrange
            var newContact = new Contact
            {
                Name = "New Contact",
                PhoneNumber = "1234567890",
                Email = "newcontact@example.com",
                Status = 1
            };

            // Mock das validações
            _contactsValidationMock.Setup(v => v.ValidateContact(newContact));

            // Mock do repositório para retornar um contato existente com o mesmo número de telefone
            var existingContact = new Contact { PhoneNumber = newContact.PhoneNumber };
            _contactsRepositoryMock.Setup(r => r.GetByPhoneNumber(newContact.PhoneNumber)).ReturnsAsync(existingContact);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.Create(newContact));
            Assert.Equal("Phone Number alredy exists", exception.Message);
        }

        [Fact]
        public async Task Create_ShouldThrowInvalidOperationException_WhenEmailAlreadyExists()
        {
            // Arrange
            var newContact = new Contact
            {
                Name = "New Contact",
                PhoneNumber = "1234567890",
                Email = "newcontact@example.com",
                Status = 1
            };

            // Mock das validações
            _contactsValidationMock.Setup(v => v.ValidateContact(newContact));

            // Mock do repositório para retornar null para telefone
            _contactsRepositoryMock.Setup(r => r.GetByPhoneNumber(newContact.PhoneNumber)).ReturnsAsync((Contact)null);

            // Mock do repositório para retornar um contato existente com o mesmo e-mail
            var existingContact = new Contact { Email = newContact.Email };
            _contactsRepositoryMock.Setup(r => r.GetByEmail(newContact.Email)).ReturnsAsync(existingContact);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.Create(newContact));
            Assert.Equal("Email alredy exists", exception.Message);
        }

        [Fact]
        public async Task Delete_ShouldReturnTrue_WhenContactExistsAndIsActive()
        {
            // Arrange
            int contactId = 1;
            var contact = new Contact
            {
                Id = contactId,
                Name = "Test Contact",
                PhoneNumber = "1234567890",
                Email = "test@example.com",
                Status = 1 // Ativo
            };

            // Mock do repositório para retornar o contato existente
            _contactsRepositoryMock.Setup(r => r.GetById(contactId)).ReturnsAsync(contact);

            // Mock do UpdateStatusAsync para retornar true
            _contactsRepositoryMock.Setup(r => r.UpdateStatusAsync(contactId)).ReturnsAsync(true);

            // Act
            var result = await _service.Delete(contactId);

            // Assert
            Assert.True(result);
            _contactsRepositoryMock.Verify(r => r.GetById(contactId), Times.Once);
            _contactsRepositoryMock.Verify(r => r.UpdateStatusAsync(contactId), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldThrowKeyNotFoundException_WhenContactDoesNotExist()
        {
            // Arrange
            int contactId = 500;
            _contactsRepositoryMock.Setup(r => r.GetById(contactId)).ReturnsAsync((Contact)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.Delete(contactId));
            Assert.Equal($"Contact with ID {contactId} not found.", exception.Message);
        }

        [Fact]
        public async Task Delete_ShouldThrowInvalidOperationException_WhenContactIsInactive()
        {
            // Arrange
            int contactId = 1;
            var inactiveContact = new Contact
            {
                Id = contactId,
                Name = "Inactive Contact",
                PhoneNumber = "1234567890",
                Email = "inactive@example.com",
                Status = 0
            };

            // Mock do repositório para retornar o contato inativo
            _contactsRepositoryMock.Setup(r => r.GetById(contactId)).ReturnsAsync(inactiveContact);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.Delete(contactId));
            Assert.Equal("Contact is inactive.", exception.Message);
        }

        [Fact]
        public async Task Put_ShouldReturnUpdatedContact_WhenContactExistsAndIsActive()
        {
            // Arrange
            int contactId = 1;
            var existingContact = new Contact
            {
                Id = contactId,
                Name = "Old Contact",
                PhoneNumber = "1234567890",
                Email = "old@example.com",
                Status = 1 
            };

            var updatedContact = new Contact
            {
                Id = contactId,
                Name = "Updated Contact",
                PhoneNumber = "0987654321",
                Email = "updated@example.com",
                Status = 1
            };

            // Mock do repositório para retornar o contato existente
            _contactsRepositoryMock.Setup(r => r.GetById(contactId)).ReturnsAsync(existingContact);

            // Mock da validação
            _contactsValidationMock.Setup(v => v.ValidateContact(updatedContact)); 

            // Mock do repositório para retornar o contato atualizado
            _contactsRepositoryMock.Setup(r => r.Put(contactId, updatedContact)).ReturnsAsync(updatedContact);

            // Act
            var result = await _service.Put(contactId, updatedContact);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedContact.Name, result.Name);
            Assert.Equal(updatedContact.PhoneNumber, result.PhoneNumber);
            Assert.Equal(updatedContact.Email, result.Email);

            _contactsRepositoryMock.Verify(r => r.GetById(contactId), Times.Once);
            _contactsRepositoryMock.Verify(r => r.Put(contactId, updatedContact), Times.Once);
        }

        [Fact]
        public async Task Put_ShouldThrowKeyNotFoundException_WhenContactDoesNotExist()
        {
            // Arrange
            int contactId = 500;
            var contact = new Contact
            {
                Id = contactId,
                Name = "Nonexistent Contact",
                PhoneNumber = "1234567890",
                Email = "nonexistent@example.com",
                Status = 1
            };

            // Mock do repositório para retornar null
            _contactsRepositoryMock.Setup(r => r.GetById(contactId)).ReturnsAsync((Contact)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.Put(contactId, contact));
            Assert.Equal($"Contact with ID {contactId} not found.", exception.Message);
        }

        [Fact]
        public async Task Put_ShouldThrowInvalidOperationException_WhenContactIsInactive()
        {
            // Arrange
            int contactId = 1;
            var inactiveContact = new Contact
            {
                Id = contactId,
                Name = "Inactive Contact",
                PhoneNumber = "1234567890",
                Email = "inactive@example.com",
                Status = 0
            };

            var updatedContact = new Contact
            {
                Id = contactId,
                Name = "Updated Contact",
                PhoneNumber = "0987654321",
                Email = "updated@example.com",
                Status = 1 
            };

            // Mock do repositório para retornar o contato inativo
            _contactsRepositoryMock.Setup(r => r.GetById(contactId)).ReturnsAsync(inactiveContact);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.Put(contactId, updatedContact));
            Assert.Equal("Contact is inactive.", exception.Message);
        }

    }
}