using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace cqrsplayground.gateway
{
    public static class ProxyExtensions
    {
        public static IServiceCollection AddProxy(this IServiceCollection services)
        {
            return services.AddSingleton<ProxyService>();
        }

        public static void RunProxy(this IApplicationBuilder app)
        {
            app.UseMiddleware<ProxyMiddleware>();
        }
    }
}
