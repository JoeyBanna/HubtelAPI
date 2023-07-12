using AutoMapper;
using Confluent.Kafka;
using Hubtel_payment_API.Data;
using Hubtel_payment_API.KafkaConfig;
using Hubtel_payment_API.Repositories.Roles;
using Hubtel_payment_API.Repositories.Users;
using Hubtel_payment_API.Repositories.Wallets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace Hubtel_payment_API
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
            services.AddControllers();
            //Mapper mapper = MappingConfig.RegisterMaps().CreateMapper();
            //services.AddSingleton(mapper);
           //services.Configure<ProducerConfig>(builConfiguration.GetSection("Kafka"));
           
            //Kafka config
           





            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            services.AddDbContext<WalletDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("DefaultSQLConnection")));
            

            services.AddSwaggerGen();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Hubtel Wallet UI",
                    Description = "A Wallet API For Hubtel Payment UI",
                });
            });

            services.AddScoped<IRoleRepository, RoleRepository>().AddScoped<IUserRepository, UserRepository>();
                services.AddScoped<IWalletRepository, WalletRepository>();
          //  services.AddSingleton<IKafkaConfiguration,KafkaConfigurations>();
           
        } 

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Wallet API V1");
            });

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
