using Microsoft.Extensions.DependencyInjection;

namespace WebAPI;
public static class DependencyInjection
{
    public static IServiceCollection AddWeb(this IServiceCollection services)
    {
        return services;
    }
}
