using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiRaquets.Entities;
using WebApiRaquets.Services;

namespace WebApiRaquets.Controllers
{
    [ApiController]
    [Route("api/raquets")]//http://localhost:8000/api/raquets
    public class RaquetController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IService service;
        private readonly ServiceTransient serviceTransient;
        private readonly ServiceScoped serviceScoped;
        private readonly ServiceSingleton serviceSingleton;
        private readonly ILogger<RaquetController> logger;

        public RaquetController(ApplicationDbContext context,IService service, ServiceTransient serviceTransient, ServiceScoped serviceScoped,
            ServiceSingleton serviceSingleton, ILogger<RaquetController> logger)
        {
            this.dbContext = context;
            this.service = service;
            this.serviceTransient = serviceTransient;
            this.serviceScoped = serviceScoped;
            this.serviceSingleton = serviceSingleton;
            this.logger = logger;
        }
        [HttpGet("GUID")]
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
            logger.LogInformation("Se obtiene el listado de raquetas");
            logger.LogWarning("Mensaje de prueba warning");
            service.EjecutarJob();
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

        [HttpGet("{name}")]
        public async Task<ActionResult<Raquet>> Get([FromRoute] string name) 
        {
            var raquet = await dbContext.Raquets.FirstOrDefaultAsync(x => x.Name.Contains(name));

            if (raquet == null) 
            {
                return NotFound();
            }
            return raquet;
        }



        [HttpPost]
        public async Task<ActionResult> Post([FromBody]Raquet raquet)
        {
            var raquetExist= await dbContext.Raquets.AnyAsync(x => x.Name == raquet.Name);

            if (raquetExist)
            {
                return BadRequest("This raquet alredy exist");
            }

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
