using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using CodeFlowMvc.Configuration;
using CodeFlowMvc.Data;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace CodeFlowMvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")))
                    .AddDatabaseDeveloperPageExceptionFilter()
                    .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddAuthentication(options => {
                //options.DefaultScheme = IndiceDefaults.AuthenticationScheme; /* Add this to make external login the default way to sign in. */;
            })
            .AddOpenIdConnect(authenticationScheme: IndiceDefaults.AuthenticationScheme, displayName: "Connect with Indice", options => {
                var indiceAuthSection = Configuration.GetSection("Auth:Indice");
                options.SignInScheme = IdentityConstants.ExternalScheme; /* This is the default external scheme when using AddDefaultIdentity<TUser>. */
                options.Authority = indiceAuthSection.GetValue<string>("Authority");
                options.ClientId = indiceAuthSection.GetValue<string>("ClientId");
                options.ClientSecret = indiceAuthSection.GetValue<string>("ClientSecret");
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.UsePkce = true;
                options.SaveTokens = true;
                options.CallbackPath = "/signin-indice";
                var scopes = indiceAuthSection.GetSection("Scopes").Get<string[]>();
                foreach (var scope in scopes) {
                    options.Scope.Add(scope);
                }
                options.Events = new OpenIdConnectEvents { 
                    OnTicketReceived = context => {
                        return Task.CompletedTask;
                    }
                };
            });
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            } else {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}