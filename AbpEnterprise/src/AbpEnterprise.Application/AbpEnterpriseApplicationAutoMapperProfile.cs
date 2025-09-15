using AutoMapper;
using AbpEnterprise.Books;

namespace AbpEnterprise;

public class AbpEnterpriseApplicationAutoMapperProfile : Profile
{
    public AbpEnterpriseApplicationAutoMapperProfile()
    {
        CreateMap<Book, BookDto>();
        CreateMap<CreateUpdateBookDto, Book>();
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
    }
}
