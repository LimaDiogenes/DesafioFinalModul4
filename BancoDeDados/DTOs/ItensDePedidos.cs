using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DesafioFinal.BancoDeDados.DTOs
{
    public class ItensDePedidos
    {
        [Key]
        [JsonPropertyName("order_items_id")]
        public int order_items_id { get; set; }
        [JsonPropertyName("order_id")]
        public int order_id { get; set; }
        [JsonPropertyName("product_id")]
        public int product_id { get; set; }
        [JsonPropertyName("quantity")]
        public int quantity { get; set; }
        [JsonPropertyName("price")]
        public double price { get; set; }

        public ItensDePedidos(int order_items_id, int order_id, int product_id, int quantity, double price)
        {
            this.order_items_id = order_items_id;
            this.order_id = order_id;
            this.product_id = product_id;
            this.quantity = quantity;
            this.price = price;
        }
    }
}
