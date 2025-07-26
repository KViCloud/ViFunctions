using System.Net;

namespace ViFunction.EdgeRouter.Extensions;

public static class RegisterExtensions
{
    public static void RegisterAppServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddReverseProxy();
        builder.Services.AddHttpForwarder();
        builder.Services.AddSingleton<HttpMessageInvoker>(_ =>
            new HttpMessageInvoker(new SocketsHttpHandler
            {
                UseProxy = false,
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.None,
                UseCookies = false
            })
        );
    }
}