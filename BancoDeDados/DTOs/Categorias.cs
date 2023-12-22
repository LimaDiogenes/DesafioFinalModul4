using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DesafioFinal.BancoDeDados.DTOs
{
    public class Categorias
    {        
        [Key]
        [JsonPropertyName("category_id")]
        public int category_id { get; set; }
        [JsonPropertyName("category_name")]
        public string category_name { get; set;}

        public Categorias(int category_id, string category_name)
        {
            this.category_id = category_id;
            this.category_name = category_name;
        }
    }
}
