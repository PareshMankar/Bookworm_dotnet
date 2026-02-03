using BookWormNET.Data;
using BookWormNET.Services.Implementation;
using BookWormNET.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookWormNET
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Controllers + JSON
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler =
                        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                });

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // DbContext
            builder.Services.AddDbContext<BookwormDbContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("BookwormDB");

                options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString)
                );
            });

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            // 🔥 SERVICE REGISTRATIONS (IMPORTANT)
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ILibraryPackageService, LibraryPackageService>();
            builder.Services.AddScoped<ILibraryCheckoutService, LibraryCheckoutService>();
            builder.Services.AddScoped<IAuthorService, AuthorService>();

            var app = builder.Build();

            // Pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowFrontend");

            // app.UseAuthentication(); // enable only when JWT/Auth is added
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
