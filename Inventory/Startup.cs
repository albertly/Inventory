using AutoMapper;
using Inventory.DBContexts;
using Inventory.Services;
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
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory
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
            var jwt = new JWTService("TW9zaGVFcmV6UHJpdmF0ZUtleQ==");

            services.AddAuthentication(options =>
           {
               options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;               
           })
            .AddJwtBearer(options =>
           {

               options.TokenValidationParameters = jwt.GetTokenValidationParameters();

               options.Events = new JwtBearerEvents
               {
                   OnChallenge = context =>
                   {
                       Console.WriteLine("OnChallenge: " + context);
                       return Task.CompletedTask;
                   },
                   OnMessageReceived = context =>
                   {
                       Console.WriteLine("OnMessageReceived: " + context);
                       return Task.CompletedTask;
                   },
                   OnForbidden = context =>
                   {
                       Console.WriteLine("OnForbidden: " +
                           context);
                       return Task.CompletedTask;
                   },
                   OnAuthenticationFailed = context =>
                   {
                       Console.WriteLine("OnAuthenticationFailed: " +
                           context.Exception.Message);
                       return Task.CompletedTask;
                   },
                   OnTokenValidated = context =>
                   {
                       Console.WriteLine("OnTokenValidated: " +
                           context.SecurityToken);
                       return Task.CompletedTask;
                   }
               };

               options.RequireHttpsMetadata = false;
           });

            services.AddAuthorization(options =>
           {
               options.AddPolicy("Manager",
                   policy => policy.RequireClaim("Job",  "CTO", "CEO", "TeamLeader" )) ;

           });

            services.AddDbContext<SQLContext>(options =>
            {
                options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=InventoryDB;Trusted_Connection=True;");
                //options.UseSqlServer(Configuration.GetConnectionString("albertConString"))
                //options.UseSqlServer(Configuration.GetConnectionString("dudiConString"));
            });

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Inventory", Version = "v1" });
            });

            services.AddScoped<IAuthService>(sp => new JWTService("TW9zaGVFcmV6UHJpdmF0ZUtleQ=="));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


            app.UseCors("MyPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Inventory v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
