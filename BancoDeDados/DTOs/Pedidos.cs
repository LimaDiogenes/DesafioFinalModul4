using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DesafioFinal.BancoDeDados.DTOs
{
    public class Pedidos
    {
        //order_id,customer_id,order_date,total_amount
        [Key]
        [JsonPropertyName("order_id")]
        public int order_id { get; set; }
        [JsonPropertyName("customer_id")]
        public int customer_id { get; set; }
        [JsonPropertyName("order_date")]
        public DateTime order_date { get; set; }
        [JsonPropertyName("total_amount")]
        public double total_amount { get; set; }

        public Pedidos(int order_id, int customer_id, DateTime order_date, double total_amount)
        {
            this.order_id = order_id;
            this.customer_id = customer_id;
            this.order_date = order_date;
            this.total_amount = total_amount;
        }
    }
}
