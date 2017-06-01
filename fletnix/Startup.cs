﻿﻿using System.Linq;
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

           
            if (_env.IsDevelopment())
            {
              
                services.AddDistributedRedisCache(options =>
                {
                    options.InstanceName = "Fletnix";
                    options.Configuration = "127.0.0.1";
                });

                services.AddScoped<IMailService, DebugMailService>();
                services.AddSingleton<IRedisCache, RedisCache>();
            } else {
				services.AddScoped<IMailService, DebugMailService>();
                services.AddSingleton<IRedisCache, MemoryCache>();
                // Implement real service
            }
            
            services.AddSession();

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

            app.UseSession();
            
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
                Authority = Configuration["AuthServer"],
                RequireHttpsMetadata = _env.IsDevelopment() ? false : true,
                ClientId = "fletnix",
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
                config.CreateMap<MovieReviewViewModel, MovieReview>().ReverseMap();
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
