using System.ComponentModel.DataAnnotations.Schema;

namespace PostoConfia.Models
{
    [Table("PRECO")]
    public class Preco
    {
        // Chaves Estrangeiras (que formam a Chave Primária Composta)
        [Column("id_posto")]
        public int PostoId { get; set; }

        [Column("id_combustivel")]
        public int CombustivelId { get; set; }

        [Column("data_registro")]
        public DateTime DataRegistro { get; set; }

        [Column("valor", TypeName = "decimal(5, 2)")]
        public decimal Valor { get; set; }

        // Propriedades de Navegação
        public virtual PostoDeCombustivel? Posto { get; set; }
        public virtual Combustivel? Combustivel { get; set; }
    }
}