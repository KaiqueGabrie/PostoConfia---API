using System.ComponentModel.DataAnnotations;

namespace PostoConfia.Models.Dtos
{
    public record ComentarioDTO
    {
        [Required(ErrorMessage = "O ID do usuário é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID de usuário inválido.")]
        public int UsuarioId { get; init; }

        [Required(ErrorMessage = "O texto do comentário é obrigatório.")]
        [Length(5, 500, ErrorMessage = "O comentário deve ter entre 5 e 500 caracteres.")]
        public required string Texto { get; init; }
    }
}