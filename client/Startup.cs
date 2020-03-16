using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using client.Entities;
using client.Models;
using client.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace client
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
            services.Configure<DbSettings>(Configuration.GetSection(nameof(DbSettings)));
            services.AddSingleton<IDbSettings>(sp => sp.GetRequiredService<IOptions<DbSettings>>().Value);
            services.Configure<SyncSetting>(Configuration.GetSection(nameof(SyncSetting)));
            services.AddSingleton<ISyncSetting>(sp => sp.GetRequiredService<IOptions<SyncSetting>>().Value);
            services.AddSingleton<ClientServerContext>();
            services.AddHostedService<SyncService>();
            services.AddControllers().AddNewtonsoftJson();;
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Client API", Version = "v1" });
            });
            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Client API V1");
            });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
