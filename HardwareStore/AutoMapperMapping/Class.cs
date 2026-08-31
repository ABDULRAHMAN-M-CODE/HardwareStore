namespace HardwareStore.AutoMapperMapping
{
    using AutoMapper;
    using HardwareStore.Models;
    using HardwareStore.ViewModel.AccountViewModels;
    public class MappingProfile : Profile
    {

       
        public MappingProfile()
        {


            // ApplicationUser is source
            //User is destination
            //null-coalescing operator. was used for EmailOrPhone number.
            CreateMap<ApplicationUser, User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.EmailOrPhoneNumber, opt => opt.MapFrom(src => src.Email ?? src.PhoneNumber))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserName));


        }
    }
}
