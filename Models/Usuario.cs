using PostoConfia.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PostoConfia.Models
{
    [Table("USUARIO")]
    public class Usuario
    {
        [Column("id_usuario")]
        public int Id { get; set; }

        [Column("nome")]
        public string? Nome { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [JsonIgnore]
        [Column("senha")]
        public string? Senha { get; set; }

        [Column("data_cadastro")]
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        // Propriedades de Navegação
        [JsonIgnore]
        public ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();

        [JsonIgnore]
        public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
    }
}