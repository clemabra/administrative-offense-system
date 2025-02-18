using AutoMapper;

namespace src.Models.Wz
{
    public class WzApplicationMappingProfile : Profile
    {
        /// <summary>
        /// Represents the mapping profile for application models.
        /// </summary>
        public WzApplicationMappingProfile()
        {
            CreateMap<JsonWzEntity, DtoWzEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // ignore Id during mapping
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore()); // ignore RowVersion during initial mapping
        }
    }
}

