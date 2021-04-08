using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Financial_Accounting_Blazor.Models;
using Financial_Accounting_Blazor.UnitOfWork;
using Financial_Accounting_Blazor.Controllers;
using Microsoft.AspNetCore.Mvc;
using Financial_Accounting_Blazor.SessionStorageSingleton;
using System.Net.Http;
using System.Text.Json;
using Newtonsoft.Json.Serialization;

namespace Financial_Accounting_Blazor
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "Test API",
                    Description = "ASP.NET Core Web API"
                });
            });

            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<BalanceContextDb>(options => options.UseSqlServer(connection))
                    .AddUnitOfWork<BalanceContextDb>();
            services.AddSingleton(SessionStorage.GetInstance());


            services.AddMvc().AddNewtonsoftJson();

            services.AddServerSideBlazor();
            services.AddScoped<Transaction>();
            services.AddScoped<UseCaseResult>();
            services.AddScoped<MoneyTransactionController>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API V1");
            });


            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapRazorPages();
            });
        }
    }
}
