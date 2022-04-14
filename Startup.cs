using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using AutoMapper;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using LibAPI.Context;
using LibAPI.Entities;
using LibAPI.Facade;
using LibAPI.Mapping;
using LibAPI.Middleware;
using LibAPI.Options;
using LibAPI.Repositories;
using LibAPI.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace LibAPI
{
public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(GetSwaggerOptions());
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
#if DEBUG
            var connectionString = Configuration.GetConnectionString("TestConnection");
#else
            var connectionString = Configuration.GetConnectionString("ProdConnection");
#endif
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, builder =>
                    builder.CommandTimeout(60)));

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            #region Mapper

            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });

            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            #endregion

            #region Repository

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            #endregion

            #region Service

            services.AddScoped<IMailer, Mailer>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();

            #endregion

            #region Fasade

            services.AddScoped<AccountServiceFacade>();
            services.AddScoped<UserServiceFacade>();
            services.AddScoped<RoleServiceFacade>();

            #endregion

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });
            services.AddCors();

            services.AddSwaggerGenNewtonsoftSupport();
            services.AddMvc(option => option.EnableEndpointRouting = false)
                .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    }
                )
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddControllersAsServices();

            if (GetSwaggerOptions().IsDefault())
                Configuration.GetSection(nameof(SwaggerOptions)).Bind(_swaggerOptions);

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "LibAPI",
                    Version = "v1"
                });
                x.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });

                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        new string[] { }
                    }
                });
                
                if (File.Exists(Path.Combine(AppContext.BaseDirectory,
                    $"{Assembly.GetExecutingAssembly().GetName().Name}.XML")))
                {
                    x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                        $"{Assembly.GetExecutingAssembly().GetName().Name}.XML"));
                }
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
            }

            app.Use((context, next) =>
            {
                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                context.Response.Headers.Add("Access-Control-Allow-Headers", "content-disposition");
                context.Response.Headers.Add("Access-Control-Expose-Headers", "X-Total-Results");
                return next.Invoke();
            });

            app.UseDefaultFiles();
            // app.UseStaticFiles(new StaticFileOptions
            // {
            //     FileProvider = new PhysicalFileProvider(
            //         Path.Combine(env.ContentRootPath, "Utils/Marking/Orders/Archive")),
            //     RequestPath = "/Archives"
            // });
            
            // app.UseStaticFiles(new StaticFileOptions
            // {
            //     FileProvider = new PhysicalFileProvider(
            //         Path.Combine(Directory.GetCurrentDirectory(), "Resources/Images")),
            //     RequestPath = "/images"
            // });
            app.UseHsts();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ExceptionsMiddleware>();
            if (GetSwaggerOptions().IsDefault())
                Configuration.GetSection(nameof(SwaggerOptions)).Bind(_swaggerOptions);

            app.UseSwagger(option => { option.RouteTemplate = _swaggerOptions.JsonRoute; });
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint(_swaggerOptions.UiEndpoint, _swaggerOptions.Description);
                option.RoutePrefix = string.Empty;
            });

            app.UseCors(builder => builder.AllowAnyOrigin()
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private SwaggerOptions GetSwaggerOptions()
        {
            return _swaggerOptions ??= new SwaggerOptions();
        }

        private SwaggerOptions _swaggerOptions;
    }
}
