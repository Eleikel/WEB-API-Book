using API_Books.BookMapper;
using API_Books.Controllers;
using API_Books.Data;
using API_Books.Repository;
using API_Books.Repository.IRepository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Books
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

            services.AddDbContext<ApplicationDbContext>(Options => Options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IBookRepository, BookRepository>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("ApiBooks", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "API BOOKS",
                    Version = "0.1",
                    Description = "Backend Book",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Name = "Eleikel",                       
                    }
                });

            });


            services.AddAutoMapper(typeof(BookMappers));

            services.AddControllers();


            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            //Documentation's Api book
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {

                options.SwaggerEndpoint("/swagger/ApiBooks/swagger.json", "API BOOK");

                options.RoutePrefix = "";
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        }
    }
}
