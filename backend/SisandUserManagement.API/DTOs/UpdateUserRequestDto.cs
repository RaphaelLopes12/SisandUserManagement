using SisandUserManagement.API.Validations;
using System.ComponentModel.DataAnnotations;

namespace SisandUserManagement.API.DTOs;

public class UpdateUserRequestDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    [StringLength(100, ErrorMessage = "O e-mail deve ter no máximo 100 caracteres.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
    [StringLength(50, ErrorMessage = "O nome de usuário deve ter no máximo 50 caracteres.")]
    public string Username { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "O endereço deve ter no máximo 200 caracteres.")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
    [DataType(DataType.Date)]
    [BirthDateValidation(ErrorMessage = "A data de nascimento não pode ser maior que hoje.")]
    public DateTime BirthDate { get; set; }

    [Required(ErrorMessage = "O telefone é obrigatório.")]
    [Phone(ErrorMessage = "Número de telefone inválido.")]
    [StringLength(20, ErrorMessage = "O telefone deve ter no máximo 20 caracteres.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "O perfil do usuário é obrigatório.")]
    [StringLength(20, ErrorMessage = "O perfil do usuário deve ter no máximo 20 caracteres.")]
    [RoleValidation]
    public string Role { get; set; } = "User";
}
