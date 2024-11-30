using Microsoft.EntityFrameworkCore;
using HubbaGolfAdmin.Database;
using HubbaGolfAdmin.Services.Implements;
using HubbaGolfAdmin.Services.Interfaces;
using HubbaGolfAdmin.Services;

namespace HubbaGolf_Admin
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EComDbContext>(config => config.UseSqlServer(configuration.GetConnectionString("Default")));

            // Add services to the container.
            services.AddAutoMapper(typeof(AutoMapperConfig));
            services.AddSingleton<EnvironmentService>();
            services.AddScoped<EComDbContextExtCustom>();
            services.AddScoped<SessionStore>();
            services.AddScoped<DapperHelper>();
            services.AddScoped<IAuthService, AuthService>();

            //
            services.AddScoped<IWebService, WebService>();
            services.AddScoped<ISystemService, SystemService>();
            //---
            services.AddHttpContextAccessor();
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.Cookie.Name = "HubbaGolfAdmin";
                options.Cookie.HttpOnly = false;
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.MaxAge = TimeSpan.FromHours(8);
                options.IdleTimeout = TimeSpan.FromHours(8);
            });

            services.AddControllersWithViews();

            return services;
        }
    }
}
