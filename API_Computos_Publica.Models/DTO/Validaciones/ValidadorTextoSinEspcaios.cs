using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Computos_Publica.Models.DTO.Validaciones
{
    public class ValidadorTextoSinEspcaios : ValidationAttribute
    {
        private readonly int _requiredLength;

        public ValidadorTextoSinEspcaios(int requiredLength)
        {
            _requiredLength = requiredLength;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string str)
            {
                var trimmed = str.Replace(" ", "");
                if (trimmed.Length > _requiredLength)
                {
                    return new ValidationResult($"El número de caracteres es mayor al límite permitido ({_requiredLength})");
                }
            }

            return ValidationResult.Success;
        }
    }
}
