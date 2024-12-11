using AutoMapper;
using PhoneBookApi.DTOs;
using PhoneBookApi.Models;

namespace PhoneBookApi.Mappings
{
    public class EntitiesToDTOMappingProfile : Profile
    {
        public EntitiesToDTOMappingProfile()
        {
            CreateMap<Contact, ContactsDTO>().ReverseMap();
        }
    }
}