using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LocalCinema.Core.Services;
using LocalCinema.Core.Services.Interfaces;
using LocalCinema.Data.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace LocalCinema.Api
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
            services.Configure<KeyManager>(Configuration.GetSection("KeyManager"));

            var keyManager = new KeyManager
            {
                AccessToken = Configuration.GetSection("KeyManager:AccessToken").Value,
                Uri = Configuration.GetSection("KeyManager:Uri").Value
            };
            Configuration.Bind("KeyManager", keyManager); //inject values from appsettings; it could also be from s3/vault
            services.AddSingleton(keyManager);

            services.AddHttpClient<ICinemaCatalogManger,CinemaCatalogManager>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LocalCinema.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LocalCinema.Api v1"));
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
