using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Register the IOptions object
// builder.Services.Configure<SlackApiSettings>(
//     builder.Configuration.GetSection("SlackApi")); 

builder.Services.AddOptions<SlackApiSettings>()
    .BindConfiguration("SlackApi")
    .ValidateDataAnnotations() // <- Enable validation
    .ValidateOnStart(); // <- Validate on app start

// Explicitly register the settings object by delegating to the IOptions object
builder.Services.AddSingleton(resolver => 
        resolver.GetRequiredService<IOptions<SlackApiSettings>>().Value);

var app = builder.Build();

// app.MapGet("/", (IOptions<SlackApiSettings> options) => options.Value);
app.MapGet("/", (SlackApiSettings options) => options);

app.Run();

public class SlackApiSettings
{
    [Required, Url]
    public string WebhookUrl { get; set; }
    [Required]
    public string DisplayName { get; set; }
    public bool ShouldNotify { get; set; }
}