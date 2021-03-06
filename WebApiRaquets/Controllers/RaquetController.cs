using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiRaquets.Entities;
using WebApiRaquets.Filters;
using WebApiRaquets.Services;

namespace WebApiRaquets.Controllers
{
    [ApiController]
    [Route("api/raquets")]//http://localhost:8000/api/raquets
    //[Authorize]
    public class RaquetController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IWebHostEnvironment env;//añadi esto para escribir
        private readonly IService service;
        private readonly ServiceTransient serviceTransient;
        private readonly ServiceScoped serviceScoped;
        private readonly ServiceSingleton serviceSingleton;
        private readonly ILogger<RaquetController> logger;

        public RaquetController(ApplicationDbContext context,IService service, ServiceTransient serviceTransient, ServiceScoped serviceScoped,
            ServiceSingleton serviceSingleton, ILogger<RaquetController> logger, IWebHostEnvironment env)//aca tambien 
        {
            this.dbContext = context;
            this.service = service;
            this.serviceTransient = serviceTransient;
            this.serviceScoped = serviceScoped;
            this.serviceSingleton = serviceSingleton;
            this.logger = logger;
            this.env = env;
        }
        [HttpGet("GUID")]
        [ResponseCache(Duration = 10)]
        [ServiceFilter(typeof(FiltroDeAccion))]
        public ActionResult ObtenerGuid()
        {
            return Ok(new
            {
                RaquetControllerTransient = serviceTransient.guid,
                ServiceA_Transient = service.GetTransient(),
                RaquetControllerScoped = serviceScoped.guid,
                ServiceA_Scoped = service.GetScoped(),
                RaquetControllerSingleton = serviceSingleton.guid,
                ServiceA_Singleton = service.GetSingleton()
            });
        }




        [HttpGet]// api/raquets
        [HttpGet("list")]// api/raquet/list
        [HttpGet("/raquet-list")]// raquet-list
        //[ResponseCache(Duration = 15)]
        //[Authorize]
        public async Task<ActionResult<List<Raquet>>> Get()
        {
            //* - Niveles de logs - 
            // Critical
            // Error
            // Warning
            // Information
            // Debug
            // Trace
            // *//
           // throw new NotImplementedException();
            //logger.LogInformation("Se obtiene el listado de raquetas");
            //logger.LogWarning("Mensaje de prueba warning");
            //service.EjecutarJob();
            return await dbContext.Raquets.Include(x => x.brands).ToListAsync();
            /*  return new List<Raquet>()
              {
                  new Raquet() { Id = 1, Name = "Pro Staff"}, <- Codigo de ejemplo
                  new Raquet() { Id = 2, Name = "Blade"}
              };*/

        }

        [HttpGet("first")]//api/raquet/first
        public async Task<ActionResult<Raquet>> FirstRaquet([FromHeader] int value, [FromQuery] string raquet, [FromQuery] int alumnoId)
        {
            return await dbContext.Raquets.FirstOrDefaultAsync();
        }

        [HttpGet("first2")]

        public ActionResult<Raquet> FirstRaquet() 
        {
            return new Raquet { Name = "Ultra" };
        }

        [HttpGet("{id:int}/{param?}")]//permite obtener informacion de una marca en especifico
        //Se puede usar ? para que no sea obligatorio el parametro 
        public async Task<ActionResult<Raquet>> Get(int id, string param)
        {
            //return await dbContext.Brands.FirstOrDefaultAsync(x => x.Id == id);//permite envia el primer registro
            var raquet = await dbContext.Raquets.FirstOrDefaultAsync(x => x.Id == id);
            if (raquet == null)
            {
                return NotFound();
            }
            return raquet;
        }

        /*
         2.- Modificar controlador principal método GET (obtener por nombre) deberemos de guardar 
            en un archivo TXT (registroConsultado.txt) el registro que nos trae la base de datos 
            al ejecutar el action.
         
         */
        [HttpGet("{name}")]
        public async Task<ActionResult<Raquet>> Get([FromRoute] string name) 
        {
            var raquet = await dbContext.Raquets.FirstOrDefaultAsync(x => x.Name.Contains(name));

            if (raquet == null) 
            {
                return NotFound();
            }

            string nombreArchivo = "registroConsultado.txt";
            string date = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            string saveMsg = $" Raquet getting register \n Name: {raquet.Name}\n Date: {date}";
            var ruta = $@"{env.ContentRootPath}\wwwroot\{nombreArchivo}";
            using (StreamWriter writer = new StreamWriter(ruta, append: true)) { writer.WriteLine(saveMsg); }


            return raquet;
        }


        /*
         1.- Modificar controlador principal método POST, deberemos de guardar en un archivo TXT 
            (nuevosRegistros.txt) los datos del registro que creamos en base de datos.
         */
        [HttpPost]
        public async Task<ActionResult> Post([FromBody]Raquet raquet)
        {
            var raquetExist = await dbContext.Raquets.AnyAsync(x => x.Name == raquet.Name);

            if (raquetExist)
            {
                return BadRequest("This raquet alredy exist");
            }

            string nombreArchivo = "nuevosRegistros.txt";
            string date = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            string saveMsg = $" Raquet register \n Name: {raquet.Name}\n Date: {date}";
            var ruta = $@"{env.ContentRootPath}\wwwroot\{nombreArchivo}";
            using (StreamWriter writer = new StreamWriter(ruta, append: true)) { writer.WriteLine(saveMsg); }


            dbContext.Add(raquet);

            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]

        public async Task<ActionResult> Put(Raquet raquet, int id) 
        {
            if (raquet.Id != id) 
            {
                return BadRequest("El Id no coincide con la URL");
            }

            dbContext.Update(raquet);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]

        public async Task<ActionResult> Delete(int id) 
        { 
            var exist = await dbContext.Raquets.AnyAsync(x => x.Id == id);
            if (!exist) 
            { 
                return NotFound();
            }

            dbContext.Remove(new Raquet() 
            { 
                Id = id
            });
            await dbContext.SaveChangesAsync();
            return Ok();
        }


    }
}
