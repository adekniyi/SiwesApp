using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SiwesApp.Models;
using SiwesApp.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using SiwesApp.Interfaces;
using SiwesApp.Repositories;
using SiwesApp.Utils;

namespace SiwesApp
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
            IdentityBuilder builder = services.AddIdentityCore<User>(opt => {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 4;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
            });

            // so that we may be able to query for the users and have their roles pulled back at the same time as well
            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            builder.AddEntityFrameworkStores<ApplicationDataContext>();
            //If we used AddIdentity only in the preceeding block, we wouldn't nedd to add the following cos it would do it automatically for us
            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<User>>();
            builder.AddDefaultTokenProviders();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Secret").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/signalR/init")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddDbContext<ApplicationDataContext>(options => options.UseLazyLoadingProxies(false).UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IAuthenticationRepo, AuthenticationRepo>();
            services.AddScoped<IGlobalRepository, GlobalRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ILecturerRepo, LecturerRepository>();
            services.AddScoped<IStudentRepo, StudentRepository>();
            services.AddScoped<ISiwesAdminRepo, SiwesAdminRepository>();
            services.AddScoped<ISiwesCoordinatorRepo, SiwesCoRepository>();
            services.AddScoped<IIndustrialSupervisorRepo, IndustrialSupervisorRepo>();
            services.AddScoped<ICloudinaryRepository, CloudinaryRepository>();

            services.AddControllers();

            services.AddMvc().AddNewtonsoftJson(o => o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            string[] origins = new string[] { "http://localhost:3000", "http://localhost:4200"
            };
            services.AddCors(options => options.AddPolicy("MyPolicy",
            builder =>
            {
                builder.AllowAnyMethod().AllowAnyHeader()
                       .WithOrigins(origins)
                       .AllowCredentials();
            }));

            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));

            services.AddAutoMapper(typeof(Startup));

            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("v1", new OpenApiInfo { Title = "SiwesApp Api", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
                setupAction.IncludeXmlComments(xmlCommentPath);

                setupAction.AddSecurityDefinition("basicAuth", new OpenApiSecurityScheme
                {
                    Description = "Input your username and password to access this Api",
                    //Description =
                    //"JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "basicAuth"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("MyPolicy");


            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SiwesApp API V1");
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("swagger", "{controller=swagger}/{action=Index}");
            });
        }
    }
}
