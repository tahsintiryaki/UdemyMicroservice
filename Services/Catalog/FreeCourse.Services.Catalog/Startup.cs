using FreeCourse.Services.Catalog.Services;
using FreeCourse.Services.Catalog.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog
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

            //Bu tanýmlama ile microservis JWT Token ile koruma altýna alýndý!
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {

                options.Authority = Configuration["IdentityServerURL"];
                options.Audience = "resource_catalog"; //IdentityServer microservisi içerisinde catalog microservisi için yazan config içerisindeki deðerle ayný olmalý.
                options.RequireHttpsMetadata = false;
            });


            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICourseService, CourseService>();

            services.AddAutoMapper(typeof(Startup));//Burasý Startup'ýn baðlý olduðu projeyi tarayarak içersinden mapleme iþleminin yapýldýðý class'ý bulur ve mapleme iþlemini o class'a göre yapar.

            //services.AddControllers();

            services.AddControllers(opt =>
            {
                //Tüm kontrollerlarýn baþýna authorize attribute'ü ekler. Senin gidip tek tek yazmana gerek kalmaz!
                opt.Filters.Add(new AuthorizeFilter());
            });


            //Start
            //bu 2 kod sayesinde artýk her hangi bir class'ýn ctorunda bu interfaceyi(IDatabaseSettings) geçtiðimizde bu bize dolu bir DatabaseSettings dönecek.
            services.Configure<DatabaseSettings>(Configuration.GetSection("DatabaseSettings"));
            services.AddSingleton<IDatabaseSettings>(sp =>
            {

                return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            });
            //End

          


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FreeCourse.Services.Catalog", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FreeCourse.Services.Catalog v1"));
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
