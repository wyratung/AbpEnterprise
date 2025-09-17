using AbpEnterprise.Books;
using AbpEnterprise.Enterprises;
using AutoMapper;

namespace AbpEnterprise;

public class AbpEnterpriseApplicationAutoMapperProfile : Profile
{
    public AbpEnterpriseApplicationAutoMapperProfile()
    {
        CreateMap<Book, BookDto>();
        CreateMap<CreateUpdateBookDto, Book>();
        CreateMap<CreateEnterpriseTypeDto, EnterpriseType>();
        CreateMap<UpdateEnterpriseTypeDto, EnterpriseType>();
         /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<Author, AuthorDto>();
        CreateMap<Author, AuthorLookupDto>();

        // EnterpriseIndustry mappings
        CreateMap<EnterpriseIndustry, EnterpriseIndustryDto>()
            .ForMember(dest => dest.EnterpriseTypes, opt => opt.MapFrom(src => src.EnterpriseTypes));
        CreateMap<CreateEnterpriseIndustryDto, EnterpriseIndustry>();
        CreateMap<UpdateEnterpriseIndustryDto, EnterpriseIndustry>();

        // EnterpriseType mappings
        CreateMap<EnterpriseType, EnterpriseTypeDto>();
            
        CreateMap<CreateEnterpriseTypeDto, EnterpriseType>();
        CreateMap<UpdateEnterpriseTypeDto, EnterpriseType>();
    }
}
