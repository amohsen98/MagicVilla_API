
using MagicVilla_VillaApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MagicVilla_VillaApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(option =>
            {

                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSqlConnection"));
            });
            //Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
            //    .WriteTo.File("Log/villalogs.txt", rollingInterval : RollingInterval.Day).CreateLogger();   
            //builder.Host.UseSerilog();
           // builder.Services.AddSingleton<ILogging, Logging>();
            builder.Services.AddControllers(/*Option=> { Option.ReturnHttpNotAcceptable = true;  }*/).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
