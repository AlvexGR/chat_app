using AutoMapper;

namespace ChatApp.Apis.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateUserMap();
        }
    }
}
