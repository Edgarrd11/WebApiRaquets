namespace WebApiRaquets.Entities
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Player { get; set; }
        
        public int RaquetId { get; set; }

        public Raquet Raquet { get; set; }
    }
}
