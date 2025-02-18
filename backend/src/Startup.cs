using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using src.Models.Authentication;
using src.Models.Offenses;
using src.Models.Wz;
using Microsoft.AspNetCore.Authentication.Negotiate;
using src.Services;
using Microsoft.AspNetCore.Http.Features;

namespace src
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            // add cors policy
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", 
                    builder => 
                    {
                        builder.WithOrigins("http://localhost:8080", "http://webengineering.ins.hs-anhalt.de:11032", "http://webengineering.ins.hs-anhalt.de:11031", "http://webengineering.ins.hs-anhalt.de:11033","http://webengineering.ins.hs-anhalt.de:11034")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });

            // include entity framwork services => using SQLite db for existing DbContext
            services.AddDbContext<OffenseDbContext>(options => 
                options.UseSqlite(Configuration.GetConnectionString("OffenseDbConnection")));
            
            services.AddDbContext<WzDbContext>(options => 
                options.UseSqlite(Configuration.GetConnectionString("WzDbConnection")));
            
            // add second DbContext for SSO tokens => also using SQLite
            services.AddDbContext<SSOTokenDbContext>(options => 
                options.UseSqlite(Configuration.GetConnectionString("SSOTokenDbConnection")));

            // add Identity and configure it with the new DbContext for storing user roles and tokens
            services.AddIdentity<IdentityUser, IdentityRole>(options =>  
            {
                ////////////////// EVERY OPTION FOR TESTING PURPOSES //////////////////
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 4;
                options.User.RequireUniqueEmail = false;
            })
            .AddEntityFrameworkStores<SSOTokenDbContext>()
            .AddDefaultTokenProviders();
            
            // add JwtBearer authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key")))
                };
            });

            // add Negotiate authentication
            services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
                .AddNegotiate();
            
            // add memory cache -> used by session state to store session data in memory
            services.AddDistributedMemoryCache();

            // configure session management
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true; // prevent client-side access to the cookie
                options.Cookie.IsEssential = true;
            });

            // service will be created once per request within a scope
            services.AddScoped<AuthenticationService>();
            services.AddAutoMapper(typeof(OffenseApplicationMappingProfile));
            services.AddAutoMapper(typeof(WzApplicationMappingProfile));

            // csv service
            services.AddScoped<ICSVService, CSVService>();

            // support multiplart body data
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10 MB
            });

            // add mvc services with support for views and controllers
            services.AddControllersWithViews();
            // add data controller
            services.AddControllers();

            // add swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApplicationLogicAPI", Version = "v1"});
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // add exception handling
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            // add https redirection
            //app.UseHttpsRedirection();

            // enable cors
            app.UseCors("CorsPolicy");
            
            // add session management
            app.UseSession();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            // add swagger
            app.UseSwagger();
            app.UseSwaggerUI(c => 
            { 
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApplicationLogicAPI V1"); 
            });

            // map controllers
            app.UseEndpoints(endpoints =>
            { 
                endpoints.MapControllers(); 
            });   
        }
    }
}