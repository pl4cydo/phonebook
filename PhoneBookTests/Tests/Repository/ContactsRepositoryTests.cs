using Microsoft.EntityFrameworkCore;
using PhoneBookApi.Data;
using PhoneBookApi.Models;
using PhoneBookApi.Repositories;
using Xunit;
using Moq;

namespace PhoneBookTests.Tests.Repository
{
    public class ContactsRepositoryTests
    {
        private readonly ContactsRepository _repository;

        public ContactsRepositoryTests()
        {
            // Configurar o banco de dados em memória
            var options = new DbContextOptionsBuilder<PhoneBookContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Criar uma instância do contexto
            using (var context = new PhoneBookContext(options))
            {
                // Adicionar contatos de teste ao banco de dados em memória
                context.Contacts.AddRange(new List<Contact>
            {
                new Contact { Id = 1, Name = "John Doe", PhoneNumber = "123456789", Email = "johndoe@gmail.com", Status = 1 },
                new Contact { Id = 2, Name = "John Doe2", PhoneNumber = "987654321", Email = "johndoe2@gmail.com", Status = 1 },
            });
                context.SaveChanges();
            }

            // Instanciar o repositório com o contexto em memória
            var dbContext = new PhoneBookContext(options);
            _repository = new ContactsRepository(dbContext);
        }

        [Fact]
        public async Task GetById_ShouldReturnCorrectContact_WhenIdExists()
        {
            // Act: chamar o método a ser testado
            var result = await _repository.GetById(1);

            // Assert: verificar os resultados
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("John Doe", result.Name);
            Assert.Equal("123456789", result.PhoneNumber);
            Assert.Equal("johndoe@gmail.com", result.Email);
            Assert.Equal(1, result.Status);
        }

        [Fact]
        public async Task GetById_ShouldReturnNull_WhenIdDoesNotExist()
        {
            // Act: chamar o método a ser testado
            var result = await _repository.GetById(500);

            // Assert: verificar que o resultado é nulo
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByPhoneNumber_ShouldReturnCorrectContact_WhenPhoneNumberExists()
        {
            // Act: chamar o método a ser testado
            var result = await _repository.GetByPhoneNumber("123456789");

            // Assert: verificar os resultados
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("John Doe", result.Name);
            Assert.Equal("123456789", result.PhoneNumber);
            Assert.Equal("johndoe@gmail.com", result.Email);
            Assert.Equal(1, result.Status);
        }

        [Fact]
        public async Task GetByPhoneNumber_ShouldReturnNull_WhenPhoneNumberDoesNotExist()
        {
            // Act: chamar o método a ser testado
            var result = await _repository.GetByPhoneNumber("000000000");

            // Assert: verificar que o resultado é nulo
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByEmail_ShouldReturnCorrectContact_WhenEmailExists()
        {
            // Act: chamar o método a ser testado
            var result = await _repository.GetByEmail("johndoe@gmail.com");

            // Assert: verificar os resultados
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("John Doe", result.Name);
            Assert.Equal("123456789", result.PhoneNumber);
            Assert.Equal("johndoe@gmail.com", result.Email);
            Assert.Equal(1, result.Status);
        }

        [Fact]
        public async Task GetByEmail_ShouldReturnNull_WhenEmailDoesNotExist()
        {
            // Act: chamar o método a ser testado
            var result = await _repository.GetByEmail("nonexistentemail@gmail.com");

            // Assert: verificar que o resultado é nulo
            Assert.Null(result);
        }

        [Fact]
        public async Task GetList_ShouldReturnAllContacts_WhenContactsExist()
        {
            // Act: chamar o método a ser testado
            var result = await _repository.GetList();

            // Assert: verificar os resultados
            var contactList = result.ToList();
            Assert.NotNull(result);
            Assert.Equal(2, contactList.Count);
            Assert.Contains(contactList, c => c.Id == 1 && c.Name == "John Doe" && c.PhoneNumber == "123456789" && c.Email == "johndoe@gmail.com");
            Assert.Contains(contactList, c => c.Id == 2 && c.Name == "John Doe2" && c.PhoneNumber == "987654321" && c.Email == "johndoe2@gmail.com");
        }

        [Fact]
        public async Task GetList_ShouldReturnEmptyList_WhenNoContactsExist()
        {
            // Configurar um novo contexto vazio
            var options = new DbContextOptionsBuilder<PhoneBookContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new PhoneBookContext(options))
            {
                var emptyRepository = new ContactsRepository(context);

                // Act: chamar o método a ser testado
                var result = await emptyRepository.GetList();

                // Assert: verificar que o resultado é uma lista vazia
                Assert.NotNull(result);
                Assert.Empty(result);
            }
        }

        [Fact]
        public async Task Create_ShouldAddNewContact_WhenCalled()
        {
            // Arrange: criar um novo contato para adicionar
            var newContact = new Contact
            {
                Name = "Jane Doe",
                PhoneNumber = "555555555",
                Email = "janedoe@gmail.com"
            };

            // Act: chamar o método a ser testado
            _repository.Create(newContact);

            // Salvar as alterações no contexto
            var saveResult = await _repository.SaveAllAsync();

            // Assert: verificar se o novo contato foi adicionado
            var result = await _repository.GetList();
            var contactList = result.ToList();
            Assert.True(saveResult);
            Assert.NotNull(result);
            Assert.Equal(3, contactList.Count);
            Assert.Contains(contactList, c => c.Name == "Jane Doe" && c.PhoneNumber == "555555555" && c.Email == "janedoe@gmail.com" && c.Status == 1);
        }

        [Fact]
        public async Task Create_ShouldNotAddContact_WhenCalledWithNull()
        {
            // Act: chamar o método a ser testado com um contato nulo
            var ex = Assert.Throws<ArgumentNullException>(() => _repository.Create(null));

            // Assert: verificar que a exceção lançada é do tipo ArgumentNullException
            Assert.Equal("newContact", ex.ParamName);
        }

        [Fact]
        public async Task Put_ShouldUpdateContact_WhenContactExists()
        {
            // Arrange: criar um contato atualizado
            var updatedContact = new Contact
            {
                Name = "John Smith",
                PhoneNumber = "111222333",
                Email = "johnsmith@gmail.com"
            };

            // Act: chamar o método a ser testado
            var result = await _repository.Put(1, updatedContact);

            // Assert: verificar se o contato foi atualizado corretamente
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("John Smith", result.Name);
            Assert.Equal("111222333", result.PhoneNumber);
            Assert.Equal("johnsmith@gmail.com", result.Email);
        }

        [Fact]
        public async Task Put_ShouldThrowKeyNotFoundException_WhenContactDoesNotExist()
        {
            // Arrange: criar um contato atualizado
            var updatedContact = new Contact
            {
                Name = "Non Existent",
                PhoneNumber = "000000000",
                Email = "nonexistent@gmail.com"
            };

            // Act & Assert: verificar se o método lança a exceção
            var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() => _repository.Put(500, updatedContact));

            // Assert: verificar a mensagem da exceção
            Assert.Equal("Contact not found.", ex.Message);
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldUpdateStatus_WhenContactExists()
        {
            // Arrange: verificar o status do contato antes da atualização
            var contactId = 1;
            var contactBeforeUpdate = await _repository.GetById(contactId);

            Assert.NotNull(contactBeforeUpdate);
            Assert.Equal(1, contactBeforeUpdate.Status);

            // Act: chamar o método a ser testado
            var result = await _repository.UpdateStatusAsync(contactId);

            // Assert: verificar se o status foi atualizado corretamente
            var contactAfterUpdate = await _repository.GetById(contactId);

            Assert.True(result);
            Assert.NotNull(contactAfterUpdate);
            Assert.Equal(0, contactAfterUpdate.Status);
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldReturnFalse_WhenContactDoesNotExist()
        {
            // Act: chamar o método com um ID que não existe
            var result = await _repository.UpdateStatusAsync(99); // ID 99 não existe

            // Assert: verificar que o resultado é falso
            Assert.False(result);
        }

    }

}