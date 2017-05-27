using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using fletnix.Data.Seeds;
using fletnix.Helpers;
using fletnix.Models;
using fletnix.Models.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using fletnix.Services;
using fletnix.ViewModels;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace fletnix
{
    public class Startup
    {

        private IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
			_env = env;
		}

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton(Configuration);
            //services.AddTransient<IdentitySeedData>();
            services.AddDbContext<FLETNIXContext>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireClaim(ClaimTypes.Role, "admin"));

                options.AddPolicy("CustomerOnly", policy =>
                    policy.RequireClaim(ClaimTypes.Role, "customer","admin"));

                options.AddPolicy("FinancialOnly", policy =>
                    policy.RequireClaim(ClaimTypes.Role, "financial","admin"));
            });

            //IDENTITY INTEGRATED
            /*services.AddIdentity<ApplicationUser, IdentityRole>(config =>
                {
                    config.User.RequireUniqueEmail = true;
                    config.Password.RequiredLength = 5;
                    config.Password.RequireNonAlphanumeric = false;
                    config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
                    config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
                    {
                        OnRedirectToLogin = async ctx =>
                        {
                            if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                            {
                                ctx.Response.StatusCode = 401;
                            }
                            else
                            {
                                ctx.Response.Redirect(ctx.RedirectUri);
                            }
                            await Task.Yield();
                        }
                    };
                })
                .AddEntityFrameworkStores<FLETNIXContext>()
                .AddDefaultTokenProviders();*/


            if (_env.IsDevelopment())
            {
                services.AddScoped<IMailService, DebugMailService>();
            } else {
				services.AddScoped<IMailService, DebugMailService>();
                // Implement real service
            }

            services.AddLogging();

            services.AddScoped<IFletnixRepository, FletnixRepository>();

            // Add framework services.
            services.AddMvc(config =>
            {
                if(_env.IsProduction()) config.Filters.Add(new RequireHttpsAttribute());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            //app.UseIdentity();
            //app.UseIdentityServer();

            app.UseCookieAuthentication(new CookieAuthenticationOptions {
                AuthenticationScheme = "cookie"
            });


            /*app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions {
                ClientId = "fletnix",
                RequireHttpsMetadata = false,
                Authority = "http://localhost:5002/",
                SignInScheme = "cookie",
            });*/


            app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
            {

                AuthenticationScheme = "oidc",
                SignInScheme = "cookie",
                Authority = "http://localhost:5002/",
                RequireHttpsMetadata = false,
                ClientId = "fletnix",
                ClientSecret = "secret",
                //ResponseType = "code id_token",
                Scope = { "openid", "profile","role"},
                GetClaimsFromUserInfoEndpoint = true,
                AutomaticChallenge = true,
                AutomaticAuthenticate = true,
                ResponseType = "id_token",
                //SaveTokens = true,

                TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.Name,
                    RoleClaimType = JwtClaimTypes.Role,
                }


            });

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug(LogLevel.Information);

            Mapper.Initialize(config =>
            {
                config.CreateMap<MovieCastViewModel, MovieCast>().ReverseMap();
                config.CreateMap<MovieDirectorViewModel, MovieDirector>().ReverseMap();
                config.CreateMap<MovieAwardViewModel, MovieAward>().ReverseMap();
                config.CreateMap<MovieGenreViewModel, MovieGenre>().ReverseMap();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "admin",
                    template: "admin/{controller=Movie}/{action=Index}/{id?}");
            });

            //seeder.EnsureSeedData().Wait();
        }





    }
}
