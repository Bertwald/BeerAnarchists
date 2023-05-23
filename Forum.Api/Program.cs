
using Forum.Data;
using Forum.Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Forum.Api;

public class Program {
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<Forum.Data.ForumDbContext>(options =>
            options.UseSqlServer(connectionString));

        /*
        builder.Services.AddDefaultIdentity<Forum.Data.Models.ForumUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ForumDbContext>();
*/
        builder.Services.AddIdentityCore<ForumUser>()
            .AddRoles<IdentityRole>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ForumDbContext>();


        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //Role Tests
        /*
        builder.Services.AddAuthorization(options => {
            options.AddPolicy("AdminKrav",
                policy => policy.RequireRole("Admin"));
            options.AddPolicy("LoginKrav",
                policy => policy.RequireAuthenticatedUser());
        });
        */
        builder.Services.AddAuthorization(options => {
            options.AddPolicy("Admin", policy => policy.RequireClaim(JwtRegisteredClaimNames.Email));
        });
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                    //options => {
                    //    options.TokenValidationParameters = new TokenValidationParameters() {
                    //        ClockSkew = TokenValidationParameters.DefaultClockSkew,
                    //        ValidateAudience = false,
                    //        ValidateIssuer = false,
                    //        ValidateIssuerSigningKey = true,
                    //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ACDt1vR3lXToPQ1g3MyN"))
                    //    };
                    //}
                    );

        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment()) {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
