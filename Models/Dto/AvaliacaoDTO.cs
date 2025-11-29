using System.ComponentModel.DataAnnotations;

namespace PostoConfia.Models.Dtos
{
    public record AvaliacaoDTO
    {
        [Required(ErrorMessage = "O ID do usuário é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID de usuário inválido.")]
        public int UsuarioId { get; init; }

        [Required(ErrorMessage = "A nota (avaliação) é obrigatória.")]
        [Range(0.0, 5.0, ErrorMessage = "A nota deve estar entre 0.0 e 5.0.")]
        public decimal Nota { get; init; }
    }
}