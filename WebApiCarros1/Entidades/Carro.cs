namespace WebApiCarros1.Entidades
{
    public class Carro
    {
        public int Id { get; set; }  
        public string Nombre { get; set; }
        public List<Clase> clases { get; set; }

    }
}
