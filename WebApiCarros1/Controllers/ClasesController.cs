﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCarros1.Entidades;

namespace WebApiCarros1.Controllers
{
    [ApiController]
    [Route("api/clases")]
    public class ClasesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public ClasesController(ApplicationDbContext context)
        {
            this.dbContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Clase>>> GetAll()
        {
            return await dbContext.Clases.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Clase>> GetById(int id)
        {
            return await dbContext.Clases.FirstOrDefaultAsync(c => c.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult<Clase>> Post(Clase clase)
        {
            var existeCarro = await dbContext.Carros.AnyAsync(x => x.Id == clase.Id);

            if (!existeCarro)
            {
                return BadRequest($"No existe el carro con el número de serie: { clase.Id}");
            }
            dbContext.Add(clase);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Clase clase, int id)
        {
            var exist = await dbContext.Clases.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }

            if (clase.Id != id)
            {
                return BadRequest("El id de la clase no coincide con el establecido en la url.");
            }

            dbContext.Update(clase);
            await dbContext.SaveChangesAsync();
            return Ok();
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Clases.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("El recurso no fue encontrado.");
            }

            //var validateRelation = await dbContext.CarroClase.AnyAsync

            dbContext.Remove(new Clase { Id = id });
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
