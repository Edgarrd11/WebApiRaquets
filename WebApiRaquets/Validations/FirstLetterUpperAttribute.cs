using System.ComponentModel.DataAnnotations;

namespace WebApiRaquets.Validations
{
    public class FirstLetterUpperAttribute : ValidationAttribute //Permite implementar el metodo IsValid
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null || string.IsNullOrEmpty(value.ToString())) 
            {
                return ValidationResult.Success;
            }

            var primeraLetra = value.ToString()[0].ToString();
            
            if(primeraLetra != primeraLetra.ToUpper()) 
            {
                return new ValidationResult("First letter may upper");
            }

            return ValidationResult.Success;
        }

    }
}
