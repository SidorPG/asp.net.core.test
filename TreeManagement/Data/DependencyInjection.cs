using Microsoft.EntityFrameworkCore;

namespace Data;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDbContext(
        this IServiceCollection service,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("DefaultConnection") ??
            throw new ArgumentNullException(nameof(configuration));
        service.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

        return service;
    }
}