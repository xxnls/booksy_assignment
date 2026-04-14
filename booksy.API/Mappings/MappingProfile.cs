using AutoMapper;
using booksy.API.Models.DTOs;
using booksy.API.Models.Entities;

namespace booksy.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Hardware mappings
            CreateMap<Hardware, HardwareDto>();
            CreateMap<CreateHardwareDto, Hardware>();
            CreateMap<UpdateHardwareDto, Hardware>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // RentalRecord mappings
            CreateMap<RentalRecord, RentalRecordDto>()
                .ForMember(dest => dest.HardwareName, opt => opt.MapFrom(src => src.Hardware.Name))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email));
            CreateMap<CreateRentalRecordDto, RentalRecord>();
            CreateMap<UpdateRentalRecordDto, RentalRecord>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
