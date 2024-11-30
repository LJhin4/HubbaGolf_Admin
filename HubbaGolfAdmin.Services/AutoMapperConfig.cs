    using AutoMapper;

namespace HubbaGolfAdmin.Services
{
    public class AutoMapperConfig
    {
        public static IMapper Configure()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                // Add your library profiles
                cfg.AddProfile<AutoMapperProfile>();

                // Add other configurations as needed
            });

            return mapperConfiguration.CreateMapper();
        }
    }
}
