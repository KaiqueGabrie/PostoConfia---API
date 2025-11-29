using System.ComponentModel.DataAnnotations.Schema;

namespace PostoConfia.Models
{
    [Table("AVALIACAO")]
    public class Avaliacao
    {
        [Column("id_avaliacao")]
        public int Id { get; set; }

        [Column("nota", TypeName = "decimal(2, 1)")]
        public decimal Nota { get; set; }

        [Column("data_avaliacao")]
        public DateTime DataAvaliacao { get; set; }

        // Chaves Estrangeiras
        [Column("id_usuario")]
        public int UsuarioId { get; set; }
        [Column("id_posto")]
        public int PostoId { get; set; }

        // Propriedades de Navegação
        public virtual Usuario? Usuario { get; set; }
        public virtual PostoDeCombustivel? Posto { get; set; }
    }
}