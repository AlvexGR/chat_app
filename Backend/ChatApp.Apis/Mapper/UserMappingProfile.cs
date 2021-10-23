using ChatApp.Dtos.Models.Users;
using ChatApp.Entities.Models;

namespace ChatApp.Apis.Mapper
{
    public static class UserMappingProfile
    {
        public static void CreateUserMap(this MappingProfile mappingProfile)
        {
            mappingProfile.CreateMap<User, UserDto>();
            mappingProfile.CreateMap<User, UserResponseDto>();
            mappingProfile.CreateMap<UserDto, User>();
            mappingProfile.CreateMap<InsertUserDto, User>();
        }
    }
}
