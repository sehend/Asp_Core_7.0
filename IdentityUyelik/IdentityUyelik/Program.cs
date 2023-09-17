using IdentityUyelik.CustemValidatoins;
using IdentityUyelik.Extenions;
using IdentityUyelik.Localizasion;
using IdentityUyelik.Models;
using IdentityUyelik.OptionsModel;
using IdentityUyelik.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

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

builder.Services.AddIdentity<AppUser, AppRole>(options =>
{

    options.User.RequireUniqueEmail = true;

    options.User.AllowedUserNameCharacters = "abcçdefgðhiýjklmnoöprsþtuüvyz1234567890_";

    options.Password.RequiredLength = 6;

    options.Password.RequireNonAlphanumeric = false;

    options.Password.RequireLowercase = true;

    options.Password.RequireUppercase = false;

    options.Password.RequireDigit = false;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);

    options.Lockout.MaxFailedAccessAttempts = 3;


}).AddPasswordValidator<PasswordValidator>()
           .AddUserValidator<UserValidator>()
           .AddErrorDescriber<localizaionIdentityErrorDescriber>()
           .AddDefaultTokenProviders()
           .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddScoped<IEmailService, EmailService>();


var app = builder.Build();

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
