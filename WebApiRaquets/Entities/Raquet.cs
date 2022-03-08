using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiRaquets.Entities
{
    public class Raquet
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="{0} field is requiered")]
        [StringLength(maximumLength:50, ErrorMessage = "{0} field have more than 50 characters")]
        public string Name { get; set; }
        [Range(100,400,ErrorMessage = "{0} can´t weight over 400g")]
        [NotMapped]
        public string Weight { get; set; }
        [CreditCard]
        [NotMapped]
        public string Card { get; set; }
        [Url]
        [NotMapped]
        public string Url { get; set; }
        public List<Brand> brands { get; set; }
    }
}
