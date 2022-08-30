using ElSaiys.Helper;
using ElSaiys.Models;
using ElSaiys.Repositories;
using ElSaiys.Services;
using ElSaiys.Services.Interfaces;
using ElSaiys.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElSaiys
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
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddTransient<IMailingSrvice, MailingSrvice>();

            services.AddControllers();

            services.AddDbContext<ApplicationDbContext>(
                option => option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddAutoMapper(typeof(Startup));

            services.AddSingleton<IError, Error>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            var key = Configuration.GetValue<string>("JWTSecret");
            var keyBytes = Encoding.ASCII.GetBytes(key);

            services.AddAuthentication(op => op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(op =>
                {
                    op.SaveToken = true;
                    op.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = false,
                        ValidateIssuer = false
                    };
                });

            services.AddTransient<SeedingData>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ElSaiys", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SeedingData seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ElSaiys v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            seeder.SeedAdminUser();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
