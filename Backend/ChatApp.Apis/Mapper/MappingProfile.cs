using AutoMapper;

namespace ChatApp.Apis.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateUserMap();
        }
    }
}
