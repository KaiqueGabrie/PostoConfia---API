using PostoConfia.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PostoConfia.Models
{
    [Table("COMBUSTIVEL")]
    public class Combustivel
    {
        [Column("id_combustivel")]
        public int Id { get; set; }

        [Column("tipo")]
        public string? Tipo { get; set; }

        // Propriedades de Navegação
        [JsonIgnore]
        public ICollection<Preco> Precos { get; set; } = new List<Preco>();
    }
}