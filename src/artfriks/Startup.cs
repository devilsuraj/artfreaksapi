using System;
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
using React.AspNet;
using Microsoft.IdentityModel.Tokens;

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

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

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
                

            });
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


                         //   .RequireClientIdentification()

                         // During development, you can disable the HTTPS requirement.

                         // When request caching is enabled, authorization and logout requests
                         // are stored in the distributed cache by OpenIddict and the user agent
                         // is redirected to the same page with a single parameter (request_id).
                         // This allows flowing large OpenID Connect requests even when using
                         // an external authentication provider like Google, Facebook or Twitter.
                         //.AddSigningCertificate("Certificate.pfx")
                         //Dont delete this line D3233644E8A0882D48F4CA91CE1E281F4D344E1C
                         //DCBF6BC95C52BDE6AA1135297589A1ADB8BB7199
                       //  .AddSigningCertificate("DCBF6BC95C52BDE6AA1135297589A1ADB8BB7199", StoreName.My, StoreLocation.LocalMachine)
                       //  .DisableHttpsRequirement()
               .EnableRequestCaching()
               .RequireClientIdentification()
               
             .AddEphemeralSigningKey();

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
            services.AddReact();
            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseCors("AeonPolicy");
            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseApplicationInsightsExceptionTelemetry();
            // Initialise ReactJS.NET. Must be before static files.
            app.UseReact(config =>
            {
                // If you want to use server-side rendering of React components,
                // add all the necessary JavaScript files here. This includes
                // your components as well as all of their dependencies.
                // See http://reactjs.net/ for more information. Example:
                //config
                //  .AddScript("~/Scripts/First.jsx")
                //  .AddScript("~/Scripts/Second.jsx");

                // If you use an external build too (for example, Babel, Webpack,
                // Browserify or Gulp), you can improve performance by disabling
                // ReactJS.NET's version of Babel and loading the pre-transpiled
                // scripts. Example:
                //config
                //  .SetLoadBabel(false)
                //  .AddScriptWithoutTransform("~/Scripts/bundle.server.js");
            });
            app.UseStaticFiles();
            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                TokenValidationParameters = new TokenValidationParameters { ValidateAudience = false },
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                RequireHttpsMetadata = false,
                Audience = "http://localhost:52909/ myclient postman",
                Authority = "http://localhost:52909/",
                
                
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
        }
    }
}
