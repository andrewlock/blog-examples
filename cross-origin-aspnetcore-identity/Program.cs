using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using cross_origin_aspnetcore_identity.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// 👇 Add this
builder.Services.Configure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, 
    x => x.Cookie.SameSite = SameSiteMode.None);

builder.Services.AddRazorPages();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAndrewLock", policy
        => policy.WithOrigins("https://andrewlock.net")
            .AllowCredentials()
            .WithMethods("GET"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors();
app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.MapGet("/api", (ClaimsPrincipal user) => user.Identity?.Name ?? "<unknown>")
    .RequireCors("AllowAndrewLock");

app.Run();
