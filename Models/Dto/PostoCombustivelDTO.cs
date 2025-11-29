using System.ComponentModel.DataAnnotations;

namespace PostoConfia.Models.Dtos
{
    public record PostoCombustivelDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [Length(3, 100, ErrorMessage = "O nome deve ter no mínimo 3 e no máximo 100 caracteres.")]
        public required string Nome { get; init; }

        [Required(ErrorMessage = "O CNPJ é obrigatório.")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "CNPJ inválido (deve ter 14 dígitos).")]
        public required string Cnpj { get; init; }

        [Required(ErrorMessage = "O endereço é obrigatório.")]
        public required string Endereco { get; init; }

        [Required(ErrorMessage = "A latitude é obrigatória.")]
        public decimal Latitude { get; init; }

        [Required(ErrorMessage = "A longitude é obrigatória.")]
        public decimal Longitude { get; init; }
    }
}