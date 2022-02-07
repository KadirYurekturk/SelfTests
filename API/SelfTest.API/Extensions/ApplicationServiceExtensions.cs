using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelfTest.API.Services;
using SelfTest.Data.Contexts;

namespace SelfTest.API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            //  services.AddTransient<IBookService, BookService>();



            services.AddDbContext<TestDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DevConnection")));

            return services;
        }
    }
}
