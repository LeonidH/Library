using System.Text;
using System.Text.Json;
using Library.Core.DataServices;
using Library.Core.DataServices.Book.Services;
using Library.Core.DataServices.UserGroup.Services;
using Library.Core.Options;
using Library.Data;
using Library.Data.Entities;
using Library.Server.OData;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Query.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Library.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var tokenConfig = builder.Configuration.GetSection(nameof(TokenConfig)).Get<TokenConfig>() ?? default!;
            var connectionStr = builder.Configuration.GetConnectionString("DefaultConnection");

            // Add services to the container.
            builder.Services.AddSingleton(tokenConfig);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionStr, x => x.MigrationsAssembly("Library.Data")));

            builder.Services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddSignInManager()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>("REFRESHTOKENPROVIDER");

            builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromSeconds(tokenConfig.RefreshTokenExpireSeconds);
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ValidIssuer = tokenConfig.Issuer,
                    ValidAudience = tokenConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfig.SecretKey)),
                    ClockSkew = TimeSpan.FromSeconds(0)
                };
            });

            builder.Services.AddTransient<DbContextInitialiser>();
            builder.Services.AddScoped<BaseDataService<Book, Guid>, BookService>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services
                .AddControllers()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase)
                .AddOData(
                    options =>
                        options
                            .Count()
                            .Expand()
                            .OrderBy()
                            .Select()
                            .SetMaxTop(100)
                            .SkipToken()
                            .Filter()
                            .AddRouteComponents("odata", EdmModelBuilder.GetEdmModel())
                );


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("webAppRequests", policyBuilder =>
                {
                    policyBuilder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins(tokenConfig.Audience)
                        .AllowCredentials();
                });
            });

            builder.Services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo { Title = "App Api", Version = "v1" });
                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                config.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
            });

            var app = builder.Build();

            app.UseODataRouteDebug();
            app.UseODataQueryRequest();
            app.UseODataBatching();


            if (app.Environment.IsDevelopment())
            {
                using var scope = app.Services.CreateScope();
                var initializer = scope.ServiceProvider.GetRequiredService<DbContextInitialiser>();
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var bookService = scope.ServiceProvider.GetRequiredService<BaseDataService<Book, Guid>>();

                await initializer.InitialiseAsync();
                await userService.InitializeAsync();
                await bookService.InitializeAsync();

                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
            await app.RunAsync();
        }
    }
}