//Clase donde se configura la API
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using WebApiRaquets.Middlewares;
using WebApiRaquets.Services;
using WebApiRaquets.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApiRaquets
{
    public class Startup
    {
        public Startup(IConfiguration configuration) 
        { 
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServuces(IServiceCollection services) 
        {

            /*
            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle*/
            services.AddControllers(opciones =>
            {
                opciones.Filters.Add(typeof(FiltroDeExcepcion));
            }).AddJsonOptions(x =>
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));
            // - - - - - - - - - - - 
            services.AddTransient<IService, ServiceA>();
            services.AddTransient<ServiceTransient>();
            services.AddScoped<ServiceScoped>();
            services.AddSingleton<ServiceSingleton>();
            // - -  - - - - - - - - - -
            services.AddTransient<FiltroDeAccion>();
            services.AddHostedService<EscribirEnArchivo>();
           // services.AddHostedService<EscribirEnArchivoT3>();
            services.AddResponseCaching();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c => 
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIRaquets", Version="v1"});
            });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> serviceLogger) 
        {

            //Use me permite agregar mi propio proceso sin afectar a los demas como Run
            //app.Use(async (context, siguiente) =>
            //{
            //    using (var ms = new MemoryStream())
            //    {
            //        //Se asigna el body del response en una variable y se le da el valor de memorystream
            //        var bodyOriginal = context.Response.Body;
            //        context.Response.Body = ms;

            //        //Permite continuar con la linea
            //        await siguiente.Invoke();

            //        //Guardamos lo que le respondemos al cliente en el string
            //        ms.Seek(0, SeekOrigin.Begin);
            //        string response = new StreamReader(ms).ReadToEnd();
            //        ms.Seek(0, SeekOrigin.Begin);

            //        //Leemos el stream y lo colocamos como estaba
            //        await ms.CopyToAsync(bodyOriginal);
            //        context.Response.Body = bodyOriginal;

            //        logger.LogInformation(response);
            //    }
            //});

            //Metodo para utilizar la clase middleware propia
            //app.UseMiddleware<ResponseHttpMiddleware>();

            //Metodo para utilizar la clase middleware sin exponer la clase. 
            app.UseResponseHttpMiddleware();

            

            app.Map("/maping", app =>
            {
                app.Run(async context =>
                {
                    await context.Response.WriteAsync("Interceptando las peticiones");
                });
            });



            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseResponseCaching();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
