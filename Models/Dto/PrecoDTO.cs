using System.ComponentModel.DataAnnotations;

namespace PostoConfia.Models.Dtos
{
    public record PrecoDTO
    {
        [Required(ErrorMessage = "O ID do combustível é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID de combustível inválido.")]
        public int CombustivelId { get; init; }

        [Required(ErrorMessage = "O valor do combustível é obrigatório.")]
        [Range(0.01, 1000.00, ErrorMessage = "Valor inválido.")]
        public decimal Valor { get; init; }
    }
}