using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WxHandle.Core.Options;

namespace WxHandle.Core
{
    public static class WxHandleExtensions
    {
        public static IApplicationBuilder UseWxHandle(this IApplicationBuilder app)
        {
            return app.UseMiddleware<WxMiddleware>();
        }

        public static IServiceCollection AddWxHandle(this IServiceCollection services, IConfigurationSection configuration)
        {
            services.Configure<WxConfig>(configuration);
            services.AddHttpClient();
            services.AddMemoryCache();
            services.AddScoped<WxXmlHelp>();
            return services;
        }
    }
}
