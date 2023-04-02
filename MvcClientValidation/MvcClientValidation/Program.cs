using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();


public class EndsWithValidationAttribute : ValidationAttribute, IClientModelValidator
{
    private readonly string _endsWith;
    public EndsWithValidationAttribute(string endsWith)
    {
        _endsWith = endsWith;
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        context.Attributes.TryAdd("data-val", "true");
        context.Attributes.TryAdd("data-val-endswith", 
            FormatErrorMessage(context.ModelMetadata.DisplayName ?? context.ModelMetadata.Name));
        context.Attributes.TryAdd("data-val-endswith-value", _endsWith);
    }

    public override string FormatErrorMessage(string name)
        => $"The field {name} must end with '{_endsWith}'";

    public override bool IsValid(object? value)
    {
        if(value is null)
        {
            return true; // Allow null values so it works with Required attribute
        }

        return value is string s&& s.EndsWith(_endsWith);
    }
}
