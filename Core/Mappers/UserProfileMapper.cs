using AutoMapper;
using TiendaOnline1.Application.Dtos;
using TiendaOnline1.Models;

namespace TiendaOnline1.Core.Mappers {
    public class UserProfileMapper : Profile
    {
        public UserProfileMapper()
        {
            CreateMap<User, UserRequestLoginDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));
            CreateMap<User, UserRequestRegisterDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }

}
