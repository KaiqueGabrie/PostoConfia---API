using System.ComponentModel.DataAnnotations;

namespace PostoConfia.Models.Dtos
{
    public record CombustivelDTO
    {
        [Required(ErrorMessage = "O tipo de combustível é obrigatório.")]
        [Length(3, 50, ErrorMessage = "O tipo deve ter entre 3 e 50 caracteres.")]
        public required string Tipo { get; init; }

        // Exemplo: "Gasolina Comum", "Etanol", "Diesel S10"
    }
}