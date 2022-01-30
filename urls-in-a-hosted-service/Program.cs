using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHostedService<TestHostedService>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();

public class TestHostedService: BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly IHostApplicationLifetime _lifetime;
    public TestHostedService(IServiceProvider services, IHostApplicationLifetime lifetime)
    {
        _services = services;
        _lifetime = lifetime;

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!await WaitForAppStartup(_lifetime, stoppingToken))
        {
            return;
        }

        PrintAddresses(_services);

        _lifetime.StopApplication();
    }

    static async Task<bool> WaitForAppStartup(IHostApplicationLifetime lifetime, CancellationToken stoppingToken)
    {
        var startedSource = new TaskCompletionSource();
        var cancelledSource = new TaskCompletionSource();
        using var reg1 = lifetime.ApplicationStarted.Register(() => startedSource.SetResult());
        using var reg2 = stoppingToken.Register(() => cancelledSource.SetResult());

        Task completedTask = await Task.WhenAny(startedSource.Task, cancelledSource.Task).ConfigureAwait(false);

        return completedTask == startedSource.Task;
    }

    void PrintAddresses(IServiceProvider services)
    {
        Console.WriteLine("Checking addresses...");
        var server = services.GetRequiredService<IServer>();
        var addressFeature = server.Features.Get<IServerAddressesFeature>();
        foreach(var address in addressFeature!.Addresses)
        {
            Console.WriteLine("Listing on address: " + address);
        }
    }

}