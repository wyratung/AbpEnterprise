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
        CreateMap<EnterpriseType, EnterpriseTypeDto>();
        CreateMap<EnterpriseIndustry, EnterpriseIndustrieDto>();
        CreateMap<CreateEnterpriseTypeDto, EnterpriseType>();
        CreateMap<UpdateEnterpriseTypeDto, EnterpriseType>();
        CreateMap<CreateEnterpriseIndustrieDto, EnterpriseIndustry>();
        CreateMap<UpdateEnterpriseIndustrieDto, EnterpriseIndustry>();
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
    }
}
