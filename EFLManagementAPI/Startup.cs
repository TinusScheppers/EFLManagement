﻿using EFLManagement.Services;
using EFLManagementAPI.Business;
using EFLManagementAPI.Context;
using EFLManagementAPI.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NJsonSchema;
using NSwag.AspNetCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;

namespace EFLManagementAPI
{
    public class Startup
    {
        private IHostingEnvironment _env;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                    //https://stackoverflow.com/questions/34753498/self-referencing-loop-detected-in-asp-net-core
                    .AddJsonOptions(options =>
                    {
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    });

#if RELEASE
            services.AddHostedService<RFIDScanner>();
#endif

            //Context
            //env in ConfigureServices? https://stackoverflow.com/questions/46871963/asp-net-core-2-how-to-access-ihostingenvironment-in-configureservices
            string connString = _env.IsDevelopment() ? Configuration["EFLManagement:MySqlConnectionString"] : Environment.GetEnvironmentVariable("CONNECTION_STRING");

            services.AddDbContextPool<EFLContext>( // replace "YourDbContext" with the class name of your DbContext
                options => options.UseMySql(connString, // replace with your Connection String
                                            mysqlOptions =>
                                            {
                                                // replace with your Server Version and Type
                                                mysqlOptions.ServerVersion(new Version(5, 6, 34), ServerType.MySql);
                                            })
                                  .EnableSensitiveDataLogging());

            // Configure CORS
            services.AddCors(corsOptions => corsOptions.AddPolicy(
                "Default",
                corsPolicyBuilder => corsPolicyBuilder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()));

            services.AddSignalR();

            //SignalR hubs
            services.AddTransient<CardHub>();
            services.AddTransient<PresenceHub>();
            services.AddTransient<BLCard>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // Enable the Swagger UI middleware and the Swagger generator
            app.UseSwaggerUi3WithApiExplorer(settings =>
            {
                settings.GeneratorSettings.DefaultPropertyNameHandling =
                    PropertyNameHandling.CamelCase;
            });

            // Configure CORS
            app.UseCors("Default");

            app.UseHttpsRedirection();
            app.UseSignalR(routes =>
            {
                routes.MapHub<CardHub>("/cardhub");
                routes.MapHub<PresenceHub>("/presencehub");
            });
            app.UseMvc();
        }
    }
}
