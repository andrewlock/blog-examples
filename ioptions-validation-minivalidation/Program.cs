using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using MiniValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<MySettings>()
    .BindConfiguration("MySettings")
    .ValidateMiniValidation() // <- Enable validation
    .ValidateOnStart(); // <- Validate on app start

// Explicitly register the settings object by delegating to the IOptions object
builder.Services.AddSingleton(resolver => 
        resolver.GetRequiredService<IOptions<MySettings>>().Value);

var app = builder.Build();

// app.MapGet("/", (IOptions<MySettings> options) => options.Value);
app.MapGet("/", (MySettings options) => options);

app.Run();

public class MySettings
{
    [Required]
    public string DisplayName { get; set; }

    [Required]
    public NestedSettings Nested { get; set; }

    public class NestedSettings
    {
        [Required]
        public string Value { get; set; }

        [Range(1, 100)]
        public int Count { get; set; }
    }
}

public static class MiniValidationExtensions
{
    public static OptionsBuilder<TOptions> ValidateMiniValidation<TOptions>(
        this OptionsBuilder<TOptions> optionsBuilder) where TOptions : class
    {
        optionsBuilder.Services.AddSingleton<IValidateOptions<TOptions>>(
            new MiniValidationValidateOptions<TOptions>(optionsBuilder.Name));
        return optionsBuilder;
    }
}

    public class MiniValidationValidateOptions<TOptions>
        : IValidateOptions<TOptions> where TOptions : class
    {
        public MiniValidationValidateOptions(string? name)
        {
            Name = name;
        }

        public string? Name { get; }

        public ValidateOptionsResult Validate(string? name, TOptions options)
        {
            // Null name is used to configure all named options.
            if (Name != null && Name != name)
            {
                // Ignored if not validating this instance.
                return ValidateOptionsResult.Skip;
            }

            // Ensure options are provided to validate against
            ArgumentNullException.ThrowIfNull(options);

            if (MiniValidator.TryValidate(options, out var validationErrors))
            {
                return ValidateOptionsResult.Success;
            }

            var typeName = options.GetType().Name;
            var errors = new List<string>();
            foreach (var (member, memberErrors) in validationErrors)
            {
                errors.Add($"DataAnnotation validation failed for '{typeName}' member: '{member}' with errors: '{string.Join("', '", memberErrors)}'.");
            }

            return ValidateOptionsResult.Fail(errors);
        }
    }
