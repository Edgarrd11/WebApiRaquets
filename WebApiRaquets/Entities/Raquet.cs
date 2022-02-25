namespace WebApiRaquets.Entities
{
    public class Raquet
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Brand> brand { get; set; }
    }
}
