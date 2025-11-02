using MyTestShop.Application;
using MyTestShop.Infrastructure;
using MyTestShop.EndPoints.Customers;

namespace MyTestShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //builder.Services.AddAuthorization();

            // Enable OpenAPI/Swagger generation
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.RegisterApplicationServices();
            builder.Services.RegisterInfrastructureServices(builder.Configuration);


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                // Enable middleware to serve generated Swagger as JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui assets (HTML, JS, CSS, etc.)
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //app.UseAuthorization();

            app.MapCustomersEndpoints();

            app.Run();
        }
    }
}
