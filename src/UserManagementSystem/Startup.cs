using FluentValidation;
using Prometheus.Client.AspNetCore;
using Prometheus.Client.DependencyInjection;
using UserManagementSystem.BLL.Services;
using UserManagementSystem.DAL.Repositories;

namespace UserManagementSystem
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
            services.AddSwaggerGen();
            services.AddMetricFactory();
            services.AddValidatorsFromAssembly(typeof(Startup).Assembly);
            services.AddScoped<UserService>();
            services.AddScoped<PhoneService>();
            services.AddScoped<UserRepository>();
            services.AddScoped<PhoneRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UsePrometheusServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
