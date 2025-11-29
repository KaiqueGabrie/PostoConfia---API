using System.ComponentModel.DataAnnotations.Schema;

namespace PostoConfia.Models
{
    [Table("COMENTARIO")]
    public class Comentario
    {
        [Column("id_comentario")]
        public int Id { get; set; }

        [Column("texto")]
        public required string Texto { get; set; }

        [Column("data_comentario")]
        public DateTime DataComentario { get; set; }

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