using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using Common;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using DataAccessLayer;
using ToDoAppWebApi.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BusinessLogicLayer.AutoMapper;
using FluentValidation.AspNetCore;
using System.Reflection;

namespace ToDoAppWebApi.Extensions
{
    public static class WebAppBuilderExtensions
    {
        public static WebApplicationBuilder AddJwtAuthentication(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer"),
                            ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience"),
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:Key"))),
                            ValidateIssuerSigningKey = true,
                            ClockSkew = TimeSpan.Zero
                        };
                    });

            return builder;
        }

        public static WebApplicationBuilder AddCorsPolicy(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://localhost:4200")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod()
                                      .AllowCredentials());
            });

            return builder;
        }
        public static WebApplicationBuilder AddRepositories(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ToDoAppContext>();
            builder.Services.AddScoped<IUserItemsRepository, UserItemsRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IItemsRepository, ItemsRepository>();
            builder.Services.AddScoped<IStatusRepository, StatusRepository>();
            builder.Services.AddScoped<IErrorLogRepository, ErrorLogRepository>();

            return builder;
        }

        public static WebApplicationBuilder AddManagers(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IItemManager, ItemManager>();
            builder.Services.AddTransient<IUserManager, UserManager>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddTransient<IErrorLogManager, ErrorLogManager>();

            return builder;
        }

        public static WebApplicationBuilder AddOtherServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<TokenGenerator>();
            builder.Services.AddTransient<ExceptionHandlingMiddleware>();

            return builder;
        }

        public static WebApplicationBuilder AddSwaggerDocumentation(this WebApplicationBuilder builder )
        {
            builder.Services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly())); 
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(Mapper));
            return builder;
        }
        public static WebApplication UseCustomSwagger(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            return app;
        }
        public static WebApplication ConfigureCustomMiddleware(this WebApplication app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseHttpsRedirection();
            app.UseCors("AllowSpecificOrigin");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            return app;
        }
    }
}
