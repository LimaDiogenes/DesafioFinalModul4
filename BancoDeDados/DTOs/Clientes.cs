using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DesafioFinal.BancoDeDados.DTOs
{
    public class Clientes
    {
        [Key]
        [JsonPropertyName("customer_id")]
        public int customer_id { get; set; }
        [JsonPropertyName("first_name")]
        public string first_name { get; set; }
        [JsonPropertyName("last_name")]
        public string last_name { get; set; }
        [JsonPropertyName("email")]
        public string email { get; set; }
        [JsonPropertyName("address")]
        public string address { get; set; }
        [JsonPropertyName("city")]
        public string city { get; set; }
        [JsonPropertyName("state")]
        public string state { get; set; }
        [JsonPropertyName("country")]
        public string country { get; set; }        


        public Clientes(int customer_id, string first_name, string last_name, string email, string address, string city, string state, string country)
        {
            this.customer_id = customer_id;
            this.first_name = first_name; 
            this.last_name = last_name;
            this.email = email;
            this.address = address;
            this.city = city;
            this.state = state;
            this.country = country;
        }
    }
}
