using Core.Models.Dtos.Auth;
using Infrastructure.Data.TMemoriesMapping;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Presentation.WebApplication.Config;
using Presentation.WebApplication.Data;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
              sqlServerOptionsAction: sqlOptions =>
              {
                  sqlOptions.EnableRetryOnFailure();
              }));

            services.AddDbContext<TMemoriesContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
              sqlServerOptionsAction: sqlOptions =>
              {
                  sqlOptions.EnableRetryOnFailure();
              }));

            services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                                      .AddEntityFrameworkStores<ApplicationDbContext>()
                                      .AddDefaultTokenProviders();

            services.AddScoped<UserManager<IdentityUser>>();
            services.AddDependencies();

            // configure strongly typed settings objects
            var jwtSection = Configuration.GetSection("JwtSettings");
            services.Configure<JwtSettingsDto>(jwtSection);
            var jwtSettings = jwtSection.Get<JwtSettingsDto>();
            var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidAudience = jwtSettings.Audience,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            }).AddFacebook(fbOptions =>
            {
                fbOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                fbOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                fbOptions.Scope.Add("user_birthday");
                fbOptions.Scope.Add("public_profile");
                fbOptions.Scope.Add("email");
                fbOptions.Fields.Add("birthday");
                fbOptions.Fields.Add("picture.width(999)");
                fbOptions.Fields.Add("gender");
                fbOptions.ClaimActions.MapCustomJson(IdentityModel.JwtClaimTypes.Picture,
                json => { return json.GetProperty("picture").GetProperty("data").GetProperty("url").GetString(); });
                
                fbOptions.SaveTokens = true;
            }).AddGoogle(gOptions =>
            {
                gOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                gOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];

                //this function is get user google profile image
                gOptions.Scope.Add("profile");
                gOptions.SignInScheme = IdentityConstants.ExternalScheme;

                gOptions.SaveTokens = true;
            });

            //Cookie Policy needed for External Auth
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
            });

            services.AddControllersWithViews();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddCors(c => { c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin()); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });

            app.UseCors(x => x
           .AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());

        }
    }
}
