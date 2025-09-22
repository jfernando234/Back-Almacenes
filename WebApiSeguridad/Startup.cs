using AutoMapper;
using Dapper.FluentMap;
using Entity.Map;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiSeguridad
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

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                // Use the MappingProfile defined in the WebApiSeguridad project
                mc.AddProfile(new MappingProfile());
            });

            FluentMapper.Initialize(config =>
            {
                config.AddMap(new ModuloMAP());
                config.AddMap(new PerfilMAP());
                config.AddMap(new PerfilModuloMAP());
                config.AddMap(new RolMAP());
                config.AddMap(new UsuarioMAP());                                
                config.AddMap(new UsuarioPerfilMAP());
                config.AddMap(new ClienteMAP());
                config.AddMap(new ProveedorMAP());
                config.AddMap(new ProductoMAP());
                //config.AddMap(new UsuarioRolMAP());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            /*************/

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiSeguridad", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
       
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiSeguridad v1"));
            }

            app.UseRouting();
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
