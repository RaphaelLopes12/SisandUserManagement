using System.ComponentModel.DataAnnotations;

namespace SisandUserManagement.API.Validations;

/// <summary>
/// Validação para garantir que a data de nascimento não seja maior que a data atual.
/// </summary>
public class BirthDateValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is DateTime date)
        {
            if (date > DateTime.UtcNow.Date)
            {
                return new ValidationResult("A data de nascimento não pode ser maior que a data atual.");
            }
            return ValidationResult.Success;
        }

        return new ValidationResult("Data de nascimento inválida.");
    }
}
