using System.ComponentModel.DataAnnotations;

namespace PostoConfia.Models.Dtos
{
    public record UsuarioDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public required string Nome { get; init; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        public required string Email { get; init; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        public required string Senha { get; init; }
    }
}