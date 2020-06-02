using BookStore.Infrastructure.Data;
using BookStore.Infrastructure.Ioc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookStore.Presentation
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private const string REPOSITORIES_NAMESPACE = "BookStore.Infrastructure.Repositories.Implementations";
        private const string SERVICES_NAMESPACE = "BookStore.Infrastructure.Services.Implementations";
        private const string MIGRATION_ASSEMBLY = "BookStore.Presentation";
        private const string CONNECTION_STRING_PATH = "ConnectionStrings:Bookstore";
        private const string ALLOWED_HOSTS = "TestEndpoints:Allowed";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; });
            services.Configure<IISServerOptions>(options => { options.AllowSynchronousIO = true; });
            services.InjectMySqlDbContext<DataContext>(Configuration[CONNECTION_STRING_PATH], MIGRATION_ASSEMBLY);
            services.InjectForNamespace(REPOSITORIES_NAMESPACE);
            services.InjectForNamespace(SERVICES_NAMESPACE); 
            
            services.AddCors(options =>
            {
                options.AddPolicy("_allow", builder =>
                {
                    builder
                        .WithOrigins(Configuration[ALLOWED_HOSTS].Split(","))
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors("_allow");

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}