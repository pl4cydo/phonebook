using Xunit;
using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PhoneBookApi.Controllers;
using PhoneBookApi.Services;
using PhoneBookApi.DTOs;
using PhoneBookApi.Models;
using PhoneBookApi.Interfaces;

namespace PhoneBookTests.Tests.Controller
{
    public class ContactsControllerTests
    {
        private readonly Mock<IContactsService> _contactsServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ContactsController _controller;
        public ContactsControllerTests()
        {
            _contactsServiceMock = new Mock<IContactsService>();
            _mapperMock = new Mock<IMapper>();
            _controller = new ContactsController(_contactsServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetById_ShouldReturnContactsDTO_WhenContactExists()
        {
            // Arrange
            var contactId = 1;

            // Mock do objeto retornado pelo serviço
            var contact = new Contact
            {
                Id = contactId,
                Name = "Test Contact",
                PhoneNumber = "5581986482982",
                Email = "placydo@gmail.com",
                Status = 1
            };

            // Mock do DTO retornado pelo mapper
            var contactDto = new ContactsDTO
            {
                Id = contactId,
                Name = "Test Contact",
                PhoneNumber = "5581986482982",
                Email = "placydo@gmail.com"
            };

            // Mock do serviço
            _contactsServiceMock.Setup(s => s.GetById(contactId)).ReturnsAsync(contact);

            // Mock do mapper
            _mapperMock.Setup(m => m.Map<ContactsDTO>(contact)).Returns(contactDto);

            // Act
            var result = await _controller.GetById(contactId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result); // Verifica se o retorno é OkObjectResult
            var returnedDto = Assert.IsType<ContactsDTO>(okResult.Value); // Verifica se o valor retornado é do tipo ContactsDTO
            Assert.Equal(contactDto.Id, returnedDto.Id); // Verifica o ID do DTO retornado
            Assert.Equal(contactDto.Name, returnedDto.Name); // Verifica o nome do DTO retornado
        }

        [Fact]
        public async Task GetList_ReturnsOkResult_WithListOfContactsDTO()
        {
            // Arrange
            var contactsList = new List<Contact>
            {
                new Contact { Id = 1, Name = "John Doe", PhoneNumber = "123456789", Email = "johndoe@gmail.com", Status = 1 },
                new Contact { Id = 2, Name = "Jane Doe2", PhoneNumber = "987654321", Email = "johndoe2@gmail.com", Status = 1 }
            };

            var contactsDTOs = new List<ContactsDTO>
            {
                new ContactsDTO { Id = 1, Name = "John Doe", PhoneNumber = "123456789", Email = "johndoe@gmail.com" },
                new ContactsDTO { Id = 2, Name = "Jane Doe2", PhoneNumber = "987654321", Email = "johndoe2@gmail.com" }
            };

            _contactsServiceMock
                .Setup(service => service.GetList())
                .ReturnsAsync(contactsList);

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<ContactsDTO>>(contactsList))
                .Returns(contactsDTOs);

            // Act
            var result = await _controller.GetList();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<ContactsDTO>>(okResult.Value);

            Assert.Equal(2, returnValue.Count);
            Assert.Equal("John Doe", returnValue[0].Name);
            Assert.Equal("Jane Doe2", returnValue[1].Name);

            _contactsServiceMock.Verify(service => service.GetList(), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<IEnumerable<ContactsDTO>>(contactsList), Times.Once);
        }

        [Fact]
        public async Task Create_ReturnsOkResult_WhenCreationIsSuccessful()
        {
            // Arrange
            var newContactDTO = new ContactsDTO { Id = 1, Name = "John Doe", PhoneNumber = "123456789", Email = "johndoe@gmail.com" };
            var newContact = new Contact { Id = 1, Name = "John Doe", PhoneNumber = "123456789", Email = "johndoe@gmail.com", Status = 1 };

            _mapperMock
                .Setup(mapper => mapper.Map<Contact>(newContactDTO))
                .Returns(newContact);

            _contactsServiceMock
                .Setup(service => service.Create(newContact))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Create(newContactDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Success", okResult.Value);

            _mapperMock.Verify(mapper => mapper.Map<Contact>(newContactDTO), Times.Once);
            _contactsServiceMock.Verify(service => service.Create(newContact), Times.Once);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenCreationFails()
        {
            // Arrange
            var newContactDTO = new ContactsDTO { Id = 1, Name = "John Doe", PhoneNumber = "123456789", Email = "johndoe@gmail.com" };
            var newContact = new Contact { Id = 1, Name = "John Doe", PhoneNumber = "123456789", Email = "johndoe@gmail.com", Status = 1 };

            _mapperMock
                .Setup(mapper => mapper.Map<Contact>(newContactDTO))
                .Returns(newContact);

            _contactsServiceMock
                .Setup(service => service.Create(newContact))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Create(newContactDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error on create a contact", badRequestResult.Value);

            _mapperMock.Verify(mapper => mapper.Map<Contact>(newContactDTO), Times.Once);
            _contactsServiceMock.Verify(service => service.Create(newContact), Times.Once);
        }

        [Fact]
        public async Task Delete_ReturnsOkResult_WhenDeletionIsSuccessful()
        {
            // Arrange
            int contactId = 1;

            _contactsServiceMock
                .Setup(service => service.Delete(contactId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(contactId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);

            _contactsServiceMock.Verify(service => service.Delete(contactId), Times.Once);
        }

        [Fact]
        public async Task Delete_ReturnsBadRequest_WhenDeletionFails()
        {
            // Arrange
            int contactId = 1;

            _contactsServiceMock
                .Setup(service => service.Delete(contactId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(contactId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error on delete a contact", badRequestResult.Value);

            _contactsServiceMock.Verify(service => service.Delete(contactId), Times.Once);
        }

        [Fact]
        public async Task Put_ReturnsOkObjectResult_WhenUpdateIsSuccessful()
        {
            // Arrange
            int contactId = 1;
            var newContactDTO = new ContactsDTO
            {
                Name = "Test Contact",
                PhoneNumber = "5581986482982",
                Email = "placydo@gmail.com"
            };

            var updatedContact = new Contact
            {
                Name = "Test Contact",
                PhoneNumber = "5581986482982",
                Email = "placydo@gmail.com",
            };

            var resultContact = new Contact
            {
                Id = contactId,
                Name = "Test Contact",
                PhoneNumber = "5581986482982",
                Email = "placydo@gmail.com",
                Status = 1
            };

            var resultContactDTO = new ContactsDTO
            {
                Id = contactId,
                Name = "Test Contact",
                PhoneNumber = "5581986482982",
                Email = "placydo@gmail.com",
            };

            _mapperMock
                .Setup(mapper => mapper.Map<Contact>(newContactDTO))
                .Returns(updatedContact);

            _contactsServiceMock
                .Setup(service => service.Put(contactId, updatedContact))
                .ReturnsAsync(resultContact);

            _mapperMock
                .Setup(mapper => mapper.Map<ContactsDTO>(resultContact))
                .Returns(resultContactDTO);

            // Act
            var result = await _controller.Put(contactId, newContactDTO);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ContactsDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            Assert.NotNull(okResult.Value);
            var resultContactsDTO = Assert.IsType<ContactsDTO>(okResult.Value);

            Assert.Equal(newContactDTO.Name, resultContactsDTO.Name);
            Assert.Equal(newContactDTO.PhoneNumber, resultContactsDTO.PhoneNumber);

            _mapperMock.Verify(mapper => mapper.Map<Contact>(newContactDTO), Times.Once);
            _contactsServiceMock.Verify(service => service.Put(contactId, updatedContact), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<ContactsDTO>(resultContact), Times.Once);
        }

    }
}