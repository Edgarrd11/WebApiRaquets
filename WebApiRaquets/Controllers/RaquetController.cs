using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiRaquets.Entities;

namespace WebApiRaquets.Controllers
{
    [ApiController]
    [Route("api/raquets")]
    public class RaquetController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public RaquetController(ApplicationDbContext context) 
        { 
            this.dbContext = context;
        }

        [HttpGet]
        public async Task<List<Raquet>> Get()
        {
            return await dbContext.Raquets.ToListAsync();
            /*  return new List<Raquet>()
              {
                  new Raquet() { Id = 1, Name = "Pro Staff"}, <- Codigo de ejemplo
                  new Raquet() { Id = 2, Name = "Blade"}
              };*/

        }

        [HttpPost]
        public async Task<ActionResult> Post(Raquet raquet)
        {
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
