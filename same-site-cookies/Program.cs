using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using cross_origin_aspnetcore_identity.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add Identity services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// 👇 This sets the cookie to use SameSite=None
builder.Services.Configure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, 
    x => x.Cookie.SameSite = SameSiteMode.None);

// 👇 This configures our custom Cookie Manager
builder.Services.Configure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, 
    x => x.CookieManager = new CookieManagerWrapper());

builder.Services.AddRazorPages();

// Configure CORS 
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAndrewLock", policy
        => policy.WithOrigins("https://andrewlock.net")
            .AllowCredentials()
            .WithMethods("GET"));
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapGet("/api", (ClaimsPrincipal user) => user.Identity?.Name ?? "<unknown>")
    .RequireCors("AllowAndrewLock");

app.Run();

// 👇 The cookie manager implementation
internal sealed class CookieManagerWrapper : ICookieManager
{
    const string LegacySuffix = "-$#legacy#$";
    private readonly ICookieManager _cookies = new ChunkingCookieManager();

    public string? GetRequestCookie(HttpContext context, string key)
    {
        if (_cookies.GetRequestCookie(context, key) is { } value)
        {
            return value;
        }

        return _cookies.GetRequestCookie(context, $"{key}{LegacySuffix}");
    }

    public void AppendResponseCookie(HttpContext context, string key, string? value, CookieOptions options)
    {
        _cookies.AppendResponseCookie(context, key, value, options);

        if (options.SameSite == SameSiteMode.None)
        {
            var clonedOptions = new CookieOptions(options)
            {
                SameSite = SameSiteMode.Unspecified
            };
            _cookies.AppendResponseCookie(context, $"{key}{LegacySuffix}", value, clonedOptions);    
        }
    }

    public void DeleteCookie(HttpContext context, string key, CookieOptions options)
    {
        _cookies.DeleteCookie(context, key, options);
        if (options.SameSite == SameSiteMode.None)
        {
            var clonedOptions = new CookieOptions(options)
            {
                SameSite = SameSiteMode.Unspecified
            };
            _cookies.DeleteCookie(context, $"{key}{LegacySuffix}", clonedOptions);
        }
    }
}