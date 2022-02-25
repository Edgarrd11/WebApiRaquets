using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiRaquets.Entities;

namespace WebApiRaquets.Controllers
{
    [ApiController]
    [Route("api/brands")]
    public class BrandsController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public BrandsController(ApplicationDbContext context)
        {
            this.dbContext = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Brand>>> GetAll()
        {
            return await dbContext.Brands.ToListAsync();
        }

        [HttpGet("{id:int}")]//permite obtener informacion de una marca en especifico
        public async Task<ActionResult<Brand>> GetById(int id)
        {
            return await dbContext.Brands.FirstOrDefaultAsync(x => x.Id == id);//permite envia el primer registro
        }

        [HttpPost]
        public async Task<ActionResult> Post(Brand brand)
        {
            var brandExist = await dbContext.Brands.AnyAsync(x => x.Id == brand.RaquetId);
          
            if (!brandExist)
            {
                return BadRequest($"No existe la raqueta con el id: {brand.RaquetId}");
            }

            dbContext.Add(brand);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Brand brand, int id)
        {
            var exist = await dbContext.Brands.AnyAsync(x => x.Id == id);
           
            if (!exist)
            {
                return NotFound("La clase especificada no existe.");
            }

            if (brand.Id != id)
            {
                return BadRequest("El id de la brand no coincide con el establecido en la url. ");
            }

            dbContext.Update(brand);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]

        public async Task<ActionResult> Delete(int id) 
        {
        var exist = await dbContext.Brands.AnyAsync(x => x.Id == id);
            if (!exist) 
            {
                return NotFound("El recurso no fue encontrado");
            }
            dbContext.Remove(new Brand { Id = id });
            await dbContext.SaveChangesAsync();
            return Ok();

        }
    }
}
