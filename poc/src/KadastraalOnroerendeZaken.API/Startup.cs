using System;
using AutoMapper;
using KadastraalOnroerendeZaken.API.Contexts;
using KadastraalOnroerendeZaken.API.Domain.Factories;
using KadastraalOnroerendeZaken.API.Repositories;
using KadastraalOnroerendeZaken.API.Services.Kafka;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KadastraalOnroerendeZaken.API
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

            services.AddScoped(_ => new KadastraalOnroerendeZakenDbContext(configuration["ConnectionString:KadasterDBConnectionString"],
                                                                           environment.IsDevelopment()));
            services.AddScoped<IKadastraalOnroerendeZakenRepository, KadastraalOnroerendeZakenRepository>();
            services.AddScoped<ILaatsteEventsRepository, LaatsteEventsRepository>();
            services.AddScoped<IKadastraalOnroerendeZaakEventFactory, KadastraalOnroerendeZaakEventFactory>();

            services.AddScoped<IKadastraalOnroerendeZaakEventProducer, KadastraalOnroerendeZaakEventProducer>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
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
