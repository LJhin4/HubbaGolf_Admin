using AutoMapper;
using System.Globalization;
using HubbaGolfAdmin.Database.Dtos;
using HubbaGolfAdmin.Database.Models;

namespace HubbaGolfAdmin.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ArticleDto, Article>().ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Article, ArticleDto>().ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UserDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<MenuDto, Menu>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Menu, MenuDto>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<CategoryDto, Category>()
                .ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<PricingDto, Pricing>()
                .ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class StringToDateTimeConverter : ITypeConverter<string, DateTime?>
    {
        public DateTime? Convert(string source, DateTime? destination, ResolutionContext context)
        {
            if (string.IsNullOrWhiteSpace(source))
                return null;

            try
            {
                return DateTime.ParseExact(source, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                return null;
            }
        }
    }

    public class DateTimeToStringConverter : ITypeConverter<DateTime?, string>
    {
        public string Convert(DateTime? source, string destination, ResolutionContext context)
        {
            if (source == null)
                return string.Empty;

            return source.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
    }
}
