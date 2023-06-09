using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Forum.Data;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Forum.Data.Models;
using Forum.Data.Interfaces;

namespace BeerAnarchists;

public class Program {
    public static void Main(string[] args) {

        var builder = WebApplication.CreateBuilder(args);
        var connectionString = builder.Configuration.GetConnectionString("ForumDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ForumDbContextConnection' not found.");

        builder.Services.AddDbContext<ForumDbContext>(options => options.UseSqlServer(connectionString));

        builder.Services.AddDefaultIdentity<Forum.Data.Models.ForumUser>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ForumDbContext>();

        //builder.Services.AddIdentity<ForumUser, IdentityRole>()
        //    .AddEntityFrameworkStores<ForumDbContext>()
        //    .AddDefaultTokenProviders();

        #region Register Services
        builder.Services.AddScoped<Forum.Services.JwtTokenService>();
        builder.Services.AddScoped<Forum.Services.AdminService>();
        builder.Services.AddScoped<Forum.Data.ForumDbContext>();
        builder.Services.AddScoped<ISubforum, Forum.Services.SubforumService>();
        builder.Services.AddScoped<IForumThread, Forum.Services.ForumThreadsService>();
        builder.Services.AddScoped<IForumPost, Forum.Services.ForumPostService>();
        builder.Services.AddScoped<IUser, Forum.Services.UserService>();
        builder.Services.AddScoped<SignInManager<Forum.Data.Models.ForumUser>>();
        builder.Services.AddScoped<Forum.Services.ApiService>();
        #endregion


        builder.Services.ConfigureApplicationCookie(options => {
            // Cookie settings
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromDays(1);

            options.LoginPath = "/Identity/Account/Login";
            options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            options.SlidingExpiration = true;
        });

        builder.Services.Configure<IdentityOptions>(options =>
        {
            // Default Password settings.
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;
        });



        builder.Services.AddRazorPages();
        builder.Services.AddAuthentication()
            .AddCookie();
        builder.Services.AddAuthorization();
     




        var app = builder.Build();



        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment()) {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        //app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();

        app.MapControllers();


        var supportedCultures = new[]{
            new CultureInfo("en-US"),
        };
        app.UseRequestLocalization(new RequestLocalizationOptions {
            DefaultRequestCulture = new RequestCulture(new CultureInfo("en-US")),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        });

        app.Run();
    }
}
