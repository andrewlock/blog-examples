using FluentValidation;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Register the validator
builder.Services.AddScoped<IValidator<SlackApiSettings>, SlackApiSettingsValidator>();

builder.Services.AddOptions<SlackApiSettings>()
    .BindConfiguration("SlackApi")
    .ValidateFluentValidation() // <- Enable validation
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
    public string? WebhookUrl { get; set; }
    public string? DisplayName { get; set; }
    public bool ShouldNotify { get; set; }
}

public class SlackApiSettingsValidator : AbstractValidator<SlackApiSettings>
{
    public SlackApiSettingsValidator()
    {
        RuleFor(x => x.DisplayName).NotEmpty();
        RuleFor(x => x.WebhookUrl)
            .NotEmpty()
            // .MustAsync((_, _) => Task.FromResult(true)) 👈 can't use async validators
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .When(x => !string.IsNullOrEmpty(x.WebhookUrl));
    }
}

public static class OptionsBuilderFluentValidationExtensions
{
    /// <summary>
    /// Register this options instance for validation using FluentValidation
    /// Note that you _can't_ use async validators, or you will get an exception at runtime.
    /// </summary>
    /// <typeparam name="TOptions">The options type to be configured.</typeparam>
    /// <param name="optionsBuilder">The options builder to add the services to.</param>
    /// <returns>The <see cref="OptionsBuilder{TOptions}"/> so that additional calls can be chained.</returns>
    public static OptionsBuilder<TOptions> ValidateFluentValidation<TOptions>(this OptionsBuilder<TOptions> optionsBuilder) where TOptions : class
    {
        optionsBuilder.Services.AddSingleton<IValidateOptions<TOptions>>(
            provider => new FluentValidationOptions<TOptions>(optionsBuilder.Name, provider));
        return optionsBuilder;
    }
}

public class FluentValidationOptions<TOptions> : IValidateOptions<TOptions> where TOptions : class
{
    private readonly IServiceProvider _serviceProvider;
    private readonly string? _name;

    public FluentValidationOptions(string? name, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _name = name;
    }

    public ValidateOptionsResult Validate(string? name, TOptions options)
    {
        // Null name is used to configure all named options.
        if (_name != null && _name != name)
        {
            // Ignored if not validating this instance.
            return ValidateOptionsResult.Skip;
        }

        // Ensure options are provided to validate against
        ArgumentNullException.ThrowIfNull(options);
        
        // Validators are registered as scoped, so need to create a scope,
        // as we will be called from the root scope
        using var scope = _serviceProvider.CreateScope();
        var validator = scope.ServiceProvider.GetRequiredService<IValidator<TOptions>>();
        var results = validator.Validate(options);
        if (results.IsValid)
        {
            return ValidateOptionsResult.Success;
        }

        string typeName = options.GetType().Name;
        var errors = new List<string>();
        foreach (var result in results.Errors)
        {
            errors.Add($"Fluent validation failed for '{typeName}.{result.PropertyName}' with the error: '{result.ErrorMessage}'.");
        }

        return ValidateOptionsResult.Fail(errors);
    }
}