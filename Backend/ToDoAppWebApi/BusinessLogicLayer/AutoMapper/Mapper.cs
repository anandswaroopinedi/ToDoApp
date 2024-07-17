
using AutoMapper;

namespace BusinessLogicLayer.AutoMapper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<Models.DtoModels.ItemDto, DataAccessLayer.Entities.Item>();
            CreateMap<Models.DtoModels.ItemDto, DataAccessLayer.Entities.UserItem>();
            CreateMap<DataAccessLayer.Entities.UserItem, Models.DtoModels.ItemDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Item.Name))
                .ForMember<string>(dest => dest.Description, opt => opt.MapFrom<string>(src => src.Item.Description))
                .ForMember<string>(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.Name));
            CreateMap<Models.DtoModels.UserDto, DataAccessLayer.Entities.User>().ReverseMap();
            CreateMap<Models.DtoModels.ErrorLogDto, DataAccessLayer.Entities.ErrorLog>();
            CreateMap<Models.InputModels.Item,Models.DtoModels.ItemDto>().ReverseMap();
            CreateMap<Models.InputModels.ErrorLog,Models.DtoModels.ErrorLogDto>().ReverseMap();
            CreateMap<Models.InputModels.User,Models.DtoModels.UserDto>().ReverseMap();
        }
    }
}
