using System;
using AutoMapper;
using KadastraalOnroerendeZakenEvents.API.Context;
using KadastraalOnroerendeZakenEvents.API.Repositories;
using KadastraalOnroerendeZakenEvents.API.Services.Kafka;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KadastraalOnroerendeZakenEvents.API
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                    .AddNewtonsoftJson();

            services.AddScoped<IKadastraalOnroerendeZaakEventConsumer, KadastraalOnroerendeZaakEventConsumer>();

            services.AddScoped(_ => new ApplicationDbContext(configuration["ConnectionString:KadasterDBConnectionString"],
                                                             environment.IsDevelopment()));
            services.AddTransient<IKozTpoRepository, KozTpoRepository>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddHostedService<BackgroundConsumer>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.EnsureDatabaseCreated(env.IsDevelopment());

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
