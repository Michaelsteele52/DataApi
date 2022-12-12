using DataApi.Integrations.WebClients.Shared;
using DataApi.Middleware;
using Microsoft.Extensions.Options;

namespace DataApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IHttpClientBuilder AddHttpClient<TInterface, TImplementation, TOptions>(
        this IServiceCollection services, ConfigurationManager config, string optionsSection)
        where TImplementation : class, TInterface where TOptions : WebClientOptions where TInterface : class
    {
        services.AddOptions<TOptions>().Bind(config.GetSection(optionsSection));
        return services.AddHttpClient<TInterface, TImplementation>((sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<TOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseAddress);
            if (options.TimeoutInMilliseconds.HasValue)
                client.Timeout = TimeSpan.FromMilliseconds(options.TimeoutInMilliseconds.Value);
        });
    }

    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<ExceptionMiddleware>();
        return builder;
    }
}