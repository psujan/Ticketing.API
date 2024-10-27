using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using System.Text;
using Ticketing.API.Data;
using Ticketing.API.Repositories;
using Ticketing.API.Repositories.Auth;
using Ticketing.API.Repositories.Interfaces;
using Ticketing.API.Repositories.Interfaces.Auth;

namespace Ticketing.API.Services
{
    public static class RegisterServices
    {
        public static IServiceCollection RegisterService(this IServiceCollection services , WebApplicationBuilder builder)
        {
            //AUTH
            services.AddScoped<IUserManagerRepository, UserManagerRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddIdentity<IdentityUser , IdentityRole>()
                    .AddRoles<IdentityRole>()
                    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("Ticketing")
                    .AddEntityFrameworkStores<TicketingAuthDbContext>()
                    .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(
                options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                }
            );

            /*
             * register repositories here
             */
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IFileUploadService, FileUploadService>();

            return services;
        }
    }
}
