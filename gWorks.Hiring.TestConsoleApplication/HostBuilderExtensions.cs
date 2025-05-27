using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace gWorks.Hiring.TestConsoleApplication;

public static class HostBuilderExtensions
{
    public static Task RunApplicationWithEntryPoint<TEntryPoint>(this HostApplicationBuilder builder, Action<TEntryPoint> entryPoint)
        where TEntryPoint : class
    {
        using var host = builder.Build();
        using var scope = host.Services.CreateScope();

        var entryService = scope.ServiceProvider.GetRequiredService<TEntryPoint>();
        entryPoint(entryService);

        return host.RunAsync();
    }
}
