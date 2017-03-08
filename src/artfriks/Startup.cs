﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using artfriks.Data;
using artfriks.Models;
using artfriks.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Models;
using OpenIddict.Core;
using CryptoHelper;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using Serilog;

namespace artfriks
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();

                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
         
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddDbContext<ApplicationDbContext>(options => { 
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                options.UseOpenIddict();
              }
            );

            /*  services.AddIdentity<ApplicationUser, IdentityRole>()
                  .AddEntityFrameworkStores<ApplicationDbContext>()
                  .AddDefaultTokenProviders();*/
            services.AddIdentity<ApplicationUser, IdentityRole>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
                o.Cookies.ApplicationCookie.AutomaticChallenge = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();
            services.AddOpenIddict()
               // Register the Entity Framework stores.
               .AddEntityFrameworkCoreStores<ApplicationDbContext>()
               // Register the ASP.NET Core MVC binder used by OpenIddict.
               // Note: if you don't call this method, you won't be able to
               // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
               .AddMvcBinders()
               // Enable the token endpoint (required to use the password flow).
               .UseJsonWebTokens()
               .EnableAuthorizationEndpoint("/connect/authorize")
               .EnableLogoutEndpoint("/connect/logout")
               .EnableTokenEndpoint("/connect/verifycode")
               .EnableUserinfoEndpoint("/Account/Userinfo")
               .AllowAuthorizationCodeFlow()
               .AllowRefreshTokenFlow()
               // Allow client applications to use the grant_type=password flow.
               .AllowPasswordFlow()
               .AllowImplicitFlow()
               //Dont delete this line D3233644E8A0882D48F4CA91CE1E281F4D344E1C
               //DCBF6BC95C52BDE6AA1135297589A1ADB8BB7199
               .AddSigningCertificate("DCBF6BC95C52BDE6AA1135297589A1ADB8BB7199", StoreName.My, StoreLocation.LocalMachine)
               .DisableHttpsRequirement()
               .EnableRequestCaching()
               .RequireClientIdentification();
               
         // .AddEphemeralSigningKey();

            services.AddCors(options =>
            {
                options.AddPolicy("AeonPolicy",
                    builder => builder.AllowAnyOrigin()
                                     .AllowAnyMethod()
                                     .AllowCredentials()
                                     .WithExposedHeaders()
                                    .AllowAnyHeader());
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMvc();
            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile(Path.Combine(env.ContentRootPath, "Logs/myapp-{Date}.txt"));
            // loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddSerilog();
            loggerFactory.AddDebug();
            app.UseCors("AeonPolicy");
            app.UseApplicationInsightsRequestTelemetry();
            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();
            app.UseBrowserLink();
            app.UseApplicationInsightsExceptionTelemetry();
            app.UseStaticFiles();
            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                TokenValidationParameters = new TokenValidationParameters { ValidateAudience = false },
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                RequireHttpsMetadata = false,
                Audience = "http://base.kmtrt.in",
                Authority = "http://base.kmtrt.in",
            });
            app.UseOpenIddict();
            app.UseIdentity();
            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            using (var context = new ApplicationDbContext(
              app.ApplicationServices.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                context.Database.EnsureCreated();

                var applications = context.Set<OpenIddictApplication>();

                // Add Mvc.Client to the known applications.
                if (!applications.Any())
                {
                    applications.Add(new OpenIddictApplication
                    {
                        ClientId = "myClient",
                        ClientSecret = Crypto.HashPassword("secret_secret_secret"),
                        DisplayName = "My client application",
                        LogoutRedirectUri = "http://base.kmtrt.in",
                        RedirectUri = "http://base.kmtrt.in/signin-oidc",
                        Type = OpenIddictConstants.ClientTypes.Public

                    });
                    applications.Add(new OpenIddictApplication
                    {
                        ClientId = "myClient2",
                        ClientSecret = Crypto.HashPassword("secret_secret_secret"),
                        DisplayName = "My client application",
                        LogoutRedirectUri = "http://54.201.192.104",
                        RedirectUri = "http://54.201.192.104/signin-oidc",
                        Type = OpenIddictConstants.ClientTypes.Confidential

                    });

                    // To test this sample with Postman, use the following settings:
                    //
                    // * Authorization URL: http://localhost:54540/connect/authorize
                    // * Access token URL: http://localhost:54540/connect/token
                    // * Client ID: postman
                    // * Client secret: [blank] (not used with public clients)
                    // * Scope: openid email profile roles
                    // * Grant type: authorization code
                    // * Request access token locally: yes
                    applications.Add(new OpenIddictApplication
                    {
                        ClientId = "postman",
                        DisplayName = "Postman",
                        RedirectUri = "https://www.getpostman.com/oauth2/callback",
                        Type = OpenIddictConstants.ClientTypes.Public
                    });
                    context.SaveChanges();
                  
                }
            }
        }
    }
}
