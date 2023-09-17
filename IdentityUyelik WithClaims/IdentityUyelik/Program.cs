using IdentityUyelik.ClaimsProvder;
using IdentityUyelik.Extenions;
using IdentityUyelik.Models;
using IdentityUyelik.OptionsModel;
using IdentityUyelik.PermissionsRoot;
using IdentityUyelik.Requirements;
using IdentityUyelik.Seeds;
using IdentityUyelik.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon")));

builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{

    options.ValidationInterval = TimeSpan.FromMinutes(30);

});

builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddIDentityWithExtenions();

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<IClaimsTransformation, UserClaimProvders>();

builder.Services.AddScoped<IAuthorizationHandler, ExChangeExpireRequirementHandler>();

builder.Services.AddScoped<IAuthorizationHandler, ViolenceRequirementHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ÝstanbulPolicy", policy =>
    {

        policy.RequireClaim("City", "Ýstanbul", "Ankara");

    });

    options.AddPolicy("ExChangePolicy", policy =>
    {

        policy.AddRequirements(new ExChangeExpireRequirement());

    });

    options.AddPolicy("ViolencePolicy", policy =>
    {

        policy.AddRequirements(new ViolenceRequirement() { ThresholdAge = 18 });

    });

    options.AddPolicy("OrderPermissionReadOrDelete", policy =>
    {

        policy.RequireClaim("Permission", Permission.Order.Read);

        policy.RequireClaim("Permission", Permission.Order.Delete);

        policy.RequireClaim("Permission", Permission.Stock.Delete);

    });

    options.AddPolicy("Permission.Order.Read", policy =>
    {

        policy.RequireClaim("Permission", Permission.Order.Read);


    });

    options.AddPolicy("Permission.Order.Delete", policy =>
    {



        policy.RequireClaim("Permission", Permission.Order.Delete);



    });

    options.AddPolicy("Permission.Stock.Delete", policy =>
    {

        policy.RequireClaim("Permission", Permission.Stock.Delete);

    });
});

builder.Services.ConfigureApplicationCookie(options =>
{

    var cookieBuilder = new CookieBuilder();

    cookieBuilder.Name = "cookie";

    options.LoginPath = new PathString("/Home/SignIn");

    options.LogoutPath = new PathString("/Member/LogOut");

    options.AccessDeniedPath = new PathString("/Member/AccessDenied");

    options.Cookie = cookieBuilder;

    options.ExpireTimeSpan = TimeSpan.FromDays(60);

    options.SlidingExpiration = true;

});

var app = builder.Build();

using (var scop = app.Services.CreateScope())
{
    var roleManager = scop.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

    await PermissionSeed.Seed(roleManager);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();


app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
