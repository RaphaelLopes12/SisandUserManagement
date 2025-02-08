using AutoMapper;
using SisandUserManagement.API.DTOs;
using SisandUserManagement.Domain.Entities;
using BCryptNet = BCrypt.Net.BCrypt;

namespace SisandUserManagement.API.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterRequestDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => BCryptNet.HashPassword(src.Password)))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ =>
                TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                    TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"))
            ))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ =>
                TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                    TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"))
            ));

        CreateMap<UpdateUserRequestDto, User>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ =>
                TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                    TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"))
    ));

        CreateMap<User, UserResponseDto>();
    }
}
