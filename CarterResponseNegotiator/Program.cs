using System.Runtime.Serialization;
using Carter;
using Carter.Response;
using Microsoft.IO;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCarter(configurator: c => {
    c.WithResponseNegotiator<XmlResponseNegotiator>();
});

var app = builder.Build();

app.UseHttpLogging();

app.MapCarter();

app.Run();

public class HomeModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", (HttpResponse resp) => resp.Negotiate(new Person
        {
            FirstName = "Andrew",
            LastName = "Lock"
        }));
    }
}

public static class StreamManager
{
    // 👇 Create a shared RecyclableMemoryStreamManager instance
    public static readonly RecyclableMemoryStreamManager Instance = new();
}

public class Person
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
}

public class XmlResponseNegotiator : IResponseNegotiator
{
    public bool CanHandle(MediaTypeHeaderValue accept)
        => accept.MatchesMediaType("application/xml");

    public async Task Handle(HttpRequest req, HttpResponse res, object model, CancellationToken cancellationToken)
    {
        res.ContentType = "application/xml";

        var serializer = new DataContractSerializer(model.GetType());
        using var ms = StreamManager.Instance.GetStream();
        serializer.WriteObject(ms, model);
         ms.Position = 0;
        await ms.CopyToAsync(res.Body, cancellationToken);
    }
}
