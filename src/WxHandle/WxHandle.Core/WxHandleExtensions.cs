using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            return services;
        }
    }
}
