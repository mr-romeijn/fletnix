﻿﻿using System;
 using System.Linq;
 using System.Reflection;
 using System.Threading.Tasks;
 using fletnix.Data.Seeds;
 using fletnix.Models;
 using IdentityServer.Helpers;
using IdentityServer.Models;
 using IdentityServer4.EntityFramework.DbContexts;
 using IdentityServer4.EntityFramework.Mappers;
 using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Resources = IdentityServer.Helpers.Resources;

namespace IdentityServer
{
    public class Startup
    {

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            services.AddTransient<IdentitySeedData>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration["Database:FletnixAuth"]));

            services.AddDbContext<FLETNIXContext>(options =>
                options.UseSqlServer(Configuration["Database:Fletnix"]));

            services.AddIdentity<IdentityUser, IdentityRole>(config =>
                {
                    config.User.RequireUniqueEmail = true;
                    config.Password.RequiredLength = 5;
                    config.Password.RequireNonAlphanumeric = false;
                                  })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

    		services.AddIdentityServer()
			   .AddOperationalStore(
				   builder => builder.UseSqlServer(Configuration["Database:FletnixAuth"], options => options.MigrationsAssembly(migrationsAssembly)))
			   //.AddInMemoryClients(Clients.Get())
			   //.AddInMemoryIdentityResources(Resources.GetIdentityResources())
			   //.AddInMemoryApiResources(Resources.GetApiResources())
			   .AddConfigurationStore(
				   builder => builder.UseSqlServer(Configuration["Database:FletnixAuth"], options => options.MigrationsAssembly(migrationsAssembly)))
			   //.AddTestUsers(Users.Get())
			   .AddAspNetIdentity<IdentityUser>()
			   .AddTemporarySigningCredential();

            /* Adds IdentityServer in combi with IDENTITY, for some reason doesn't work */

            /*services.AddIdentityServer()
                .AddTemporarySigningCredential()
                .AddInMemoryIdentityResources(Resources.GetIdentityResources())
                .AddInMemoryApiResources(Resources.GetApiResources())
                .AddInMemoryClients(Clients.Get())
                .AddAspNetIdentity<IdentityUser>();*/

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IdentitySeedData seeder)
        {

            InitializeDbTestData(app);

			app.Use(async (context, next) =>
		    {
			  context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
			  context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
			  context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
			  await next();
		    });

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

           app.UseCookieAuthentication(new CookieAuthenticationOptions
           {
               AuthenticationScheme = "idsrv",
               AutomaticAuthenticate = false,
               AutomaticChallenge = false
           });

            app.UseIdentity();

            app.UseIdentityServer();

            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();

            seeder.EnsureSeedData().Wait();
        }


		private static void InitializeDbTestData(IApplicationBuilder app)
		{
			using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
			{
				scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
				scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();
				scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();

				var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

				if (!context.Clients.Any())
				{
					foreach (var client in Clients.Get())
					{
						context.Clients.Add(client.ToEntity());
					}
					context.SaveChanges();
				}

				if (!context.IdentityResources.Any())
				{
					foreach (var resource in Resources.GetIdentityResources())
					{
						context.IdentityResources.Add(resource.ToEntity());
					}
					context.SaveChanges();
				}

				if (!context.ApiResources.Any())
				{
					foreach (var resource in Resources.GetApiResources())
					{
						context.ApiResources.Add(resource.ToEntity());
					}
					context.SaveChanges();
				}

				var userManager = scope.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<IdentityUser>>();
				if (!userManager.Users.Any())
				{
					foreach (var testUser in Users.Get())
					{
						var identityUser = new IdentityUser(testUser.Username)
						{
							Id = testUser.SubjectId
						};

						foreach (var claim in testUser.Claims)
						{
							identityUser.Claims.Add(new IdentityUserClaim<string>
							{
								UserId = identityUser.Id,
								ClaimType = claim.Type,
								ClaimValue = claim.Value,
							});
						}

						userManager.CreateAsync(identityUser, "Password123!").Wait();
					}
				}
			}
		}

    }
}
