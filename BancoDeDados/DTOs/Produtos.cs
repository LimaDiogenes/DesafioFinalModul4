using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DesafioFinal.BancoDeDados.DTOs
{
    public class Produtos
    {
        [Key]
        [JsonPropertyName("product_id")]
        public int product_id { get; set; }
        [JsonPropertyName("product_name")]
        public string product_name { get; set; }
        [JsonPropertyName("description")]
        public string description { get; set; }
        [JsonPropertyName("price")]
        public double price { get; set; }
        [JsonPropertyName("category_id")]
        public int category_id { get; set; }
        [JsonPropertyName("supplier_id")]
        public int supplier_id { get; set; }

        public Produtos(int product_id, string product_name, string description, double price, int category_id, int supplier_id)
        {
            this.product_id = product_id;
            this.product_name = product_name;
            this.description = description;
            this.price = price;
            this.category_id = category_id;
            this.supplier_id = supplier_id;
        }


    }
}
