using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PhoneBookApi.DTOs;
using PhoneBookApi.Interfaces;
using PhoneBookApi.Models;

namespace PhoneBookApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ContactsController : Controller
    {
        private readonly IContactsService _contactsService;
        private readonly IMapper _mapper;

        public ContactsController(IContactsService contactsService, IMapper mapper) {
            _contactsService = contactsService;
            _mapper = mapper;
        }

        [HttpGet("list/{id}")]
        public async Task<ActionResult<ContactsDTO>> GetById(int id) {
            Contact contactResult = await _contactsService.GetById(id);
            ContactsDTO contactsDTO = _mapper.Map<ContactsDTO>(contactResult);
            return Ok(contactsDTO);
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<ContactsDTO>>> GetList()
        {
            IEnumerable<Contact> contactsList = await _contactsService.GetList();
            IEnumerable<ContactsDTO> contactsDTOs = _mapper.Map<IEnumerable<ContactsDTO>>(contactsList);
            return Ok(contactsDTOs);
        }

        [HttpPost("create")]
        public async Task<ActionResult<ResponseModel>> Create(ContactsDTO newContactDTO)
        {
            Contact newContact = _mapper.Map<Contact>(newContactDTO);
            await _contactsService.Create(newContact);
            return Ok(new ResponseModel{ response = "success" });
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            bool result = await _contactsService.Delete(id);
            return result ? Ok() : BadRequest("Error on delete a contact");
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<ContactsDTO>> Put(int id, ContactsDTO newContactDTO) {
            Contact newContact = _mapper.Map<Contact>(newContactDTO);
            Contact contactResult = await _contactsService.Put(id, newContact);
            ContactsDTO resultContactsDTO = _mapper.Map<ContactsDTO>(contactResult);
            return Ok(resultContactsDTO);
        }

    }
}