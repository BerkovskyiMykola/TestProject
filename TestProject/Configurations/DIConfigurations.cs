using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TestProject.BLL.MappingProfiles;
using TestProject.BLL.Services.JWT;
using TestProject.BLL.Services.JWT.Settings;
using TestProject.DAL.Entities;
using TestProject.Services.Mail;
using TestProject.BLL.Services.Account;
using TestProject.BLL.Services.ManageUser;
using TestProject.BLL.Services.BackupAndRestore;

namespace TestProject.Configurations
{
    public static class DIConfigurations
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddTransient<IJwtService, JwtService>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IManageUserService, ManageUserService>();
            services.AddTransient<IBackupAndRestoreService, BackupAndRestoreService>();

            return services;
        }

        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            var config = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserMappingProfile());
            });
            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "TestProject", Version = "v1" });
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "TestProject",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
            });

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = JwtSettings.ISSUER,
                            ValidateAudience = true,
                            ValidAudience = JwtSettings.AUDIENCE,
                            ValidateLifetime = true,
                            IssuerSigningKey = JwtSettings.GetSymmetricSecurityKey(),
                            ValidateIssuerSigningKey = true,
                        };
                    });

            return services;
        }
    }
}
