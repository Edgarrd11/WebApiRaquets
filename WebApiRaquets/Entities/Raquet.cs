using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiRaquets.Validations;

namespace WebApiRaquets.Entities
{
    public class Raquet
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} field is requiered")]
        [StringLength(maximumLength: 50, ErrorMessage = "{0} field have more than 50 characters")]
        [FirstLetterUpper]//Validacion personalizada

        public string Name { get; set; }
        [Range(100, 400, ErrorMessage = "{0} can´t weight over 400g")]
        [NotMapped]
        public string Weight { get; set; }
        [CreditCard]
        [NotMapped]
        public string Card { get; set; }
        [Url]
        [NotMapped]
        public string Url { get; set; }
        public List<Brand> brands { get; set; }

        [NotMapped]
        public int Menor { get; set; }
        [NotMapped]
        public int Mayor { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) 
        {
            if (!string.IsNullOrEmpty(Name)) 
            {
                var primeraLetra = Name[0].ToString();

                if(primeraLetra != primeraLetra.ToUpper()) 
                {
                    yield return new ValidationResult("First letter may upper", new String[] { nameof(Menor) });
                }
            }

            if(Menor > Mayor) 
            {
                yield return new ValidationResult("This value can´t be more bigger than the Mayor", new String[] { nameof(Menor) });
            }
        }
    }
}
