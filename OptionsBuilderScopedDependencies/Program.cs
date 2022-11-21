using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<MySettings>()
    .Configure<IServiceProvider>((opts, provider) => 
    {
        using var scope = provider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ValueService>();
        opts.MyValue = service.GetValue();
    });

builder.Services.AddScoped<ValueService>();

var app = builder.Build();

app.MapGet("/", (IOptionsSnapshot<MySettings> settings, ValueService service) => new {
    mySettings = settings.Value.MyValue,
    service = service.GetValue(),
});

app.Run();

public class ValueService
{
    private readonly Guid _val = Guid.NewGuid();

    // Return a fixed Guid for the lifetime of the service
    public Guid GetValue() => _val;
}

public class MySettings
{
    public Guid MyValue { get; set; }
}