using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCarros1.Entidades;
using WebApiCarros1.Services;

namespace WebApiCarros1.Controllers
{
    [ApiController]
    [Route("api/carros")]
    public class CarrosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        //Variables para escibir en los archivos del punto 1 y 2 de la TAREA 3, con el método GET y POST.
        private readonly IWebHostEnvironment env;

        //Constructor de CarrosController, donde entra El dbContext para la base de datos.
        //Y el IWebHostEnvironment para escribir la ruta de los archivos para poder escribir en ellos.
        public CarrosController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            this.dbContext = context;
            this.env = env;
        }

        [HttpGet]
        public async Task<ActionResult<List<Carro>>> Get()
        {
            return await dbContext.Carros.Include(x => x.clases).ToListAsync();
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<Carro>> Get([FromRoute] string nombre)
        {
            var carro = await dbContext.Carros.FirstOrDefaultAsync(x => x.Nombre.Contains(nombre));
            if (carro == null)
            {
                return NotFound("El carro no se encontró.");
            }

            //Método para poder escribir en el archivo para el Punto 2 de la TAREA 3.
            EscribirGet(nombre);

            return carro;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Carro carro)
        {
            var existeCarroMismoNombre = await dbContext.Carros.AnyAsync(x => x.Nombre == carro.Nombre);
            if (existeCarroMismoNombre)
            {
                return BadRequest("Ya existe un auto con el mismo nombre");
            }

            //Método para poder escribir el nombre en el archivo para el Punto 1 de la TAREA 3.
            EscribirPost("-->" + (carro.Nombre).ToString());

            dbContext.Add(carro);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Carro carro, int id)
        {
            var exist = await dbContext.Carros.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }

            if (carro.Id != id)
            {
                return BadRequest("El id del carro no coincide con el establecido en la url.");
            }
            dbContext.Update(carro);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Carros.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }

            dbContext.Remove(new Carro()
            {
                Id = id
            });
            await dbContext.SaveChangesAsync();
            return Ok();
        }


        //MÉTODOS PARA LOS PUNTOS 1 Y 2.
        //Estos dos se crean, si no están creados, y se guardan en la carpeta wwwroot
        private void EscribirGet(string msg)
        {
            //así se hace la ruta con el enviroment
            var ruta = $@"{env.ContentRootPath}\wwwroot\registroConsultado.txt";
            using (StreamWriter writer = new StreamWriter(ruta, append: true)) { writer.WriteLine(msg +"        "+ DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")); writer.Close(); }
        }
        private void EscribirPost(string msg)
        {
            var ruta = $@"{env.ContentRootPath}\wwwroot\nuevosRegistros.txt";
            using (StreamWriter writer = new StreamWriter(ruta, append: true)) { writer.WriteLine(msg + "        " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")); writer.Close(); }
        }
    }
}
