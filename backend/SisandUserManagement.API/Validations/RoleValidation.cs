using System.ComponentModel.DataAnnotations;

namespace SisandUserManagement.API.Validations;

public class RoleValidation : ValidationAttribute
{
    private static readonly string[] AllowedRoles = { "Admin", "User" };

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string role && AllowedRoles.Any(r => r.Equals(role, StringComparison.OrdinalIgnoreCase)))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult("O campo Role deve ser 'Admin' ou 'User' (maiúsculas ou minúsculas são aceitas).");
    }
}
