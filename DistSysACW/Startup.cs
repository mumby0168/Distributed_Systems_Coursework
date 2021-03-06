﻿using System;
using DistSysACW.Middleware;
using DistSysACW.Repositorys;
using DistSysACW.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DistSysACW
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
            services.AddDbContext<Models.UserContext>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddTransient<StatusCodeExceptionMiddleware>();

            services.AddSingleton<IRsaProvider, RsaProvider>();
            services.AddTransient<ILogRepository, LogRepository>();
                        

            services.AddMvc(options => {
                options.AllowEmptyInputInBodyModelBinding = true;
                options.Filters.Add(new Filters.AuthFilter());})
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            app.UseMiddleware<Middleware.AuthMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            
            app.UseMiddleware<Middleware.StatusCodeExceptionMiddleware>();

            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
