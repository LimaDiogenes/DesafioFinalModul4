using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DesafioFinal.BancoDeDados.DTOs
{
    public class Fornecedores
    {        
        [Key]
        [JsonPropertyName("supplier_id")]
        public int supplier_id { get; set; }
        [JsonPropertyName("supplier_name")]
        public string supplier_name { get; set; }
        [JsonPropertyName("supplier_email")]
        public string supplier_email { get; set; }
        [JsonPropertyName("supplier_phone")]
        public string supplier_phone { get; set; }
        [JsonPropertyName("supplier_address")]
        public string supplier_address { get; set; }

        public Fornecedores(int supplier_id, string supplier_name, string supplier_email, string supplier_phone, string supplier_address)
        {
            this.supplier_id = supplier_id;
            this.supplier_name = supplier_name;
            this.supplier_email = supplier_email;
            this.supplier_phone = supplier_phone;
            this.supplier_address = supplier_address;
        }

    }
}
