using PostoConfia.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PostoConfia.Models
{
    [Table("POSTO_DE_COMBUSTIVEL")]
    public class PostoDeCombustivel
    {
        [Column("id_posto")]
        public int Id { get; set; }

        [Column("nome")]
        public string? Nome { get; set; }

        [Column("cnpj")]
        public string? Cnpj { get; set; }

        [Column("endereco")]
        public string? Endereco { get; set; }

        [Column("latitude", TypeName = "decimal(10, 8)")]
        public decimal? Latitude { get; set; }

        [Column("longitude", TypeName = "decimal(11, 8)")]
        public decimal? Longitude { get; set; }

        [Column("avaliacao_media", TypeName = "decimal(2, 1)")]
        public decimal AvaliacaoMedia { get; set; }

        // Propriedades de Navegação
        [JsonIgnore]
        public ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();

        [JsonIgnore]
        public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

        [JsonIgnore]
        public ICollection<Preco> Precos { get; set; } = new List<Preco>();
    }
}