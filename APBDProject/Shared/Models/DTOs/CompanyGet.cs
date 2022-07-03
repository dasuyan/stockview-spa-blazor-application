using System.Text.Json.Serialization;

namespace APBDProject.Shared.Models.DTOs
{
    public class CompanyGet
    {
        [JsonPropertyName("ticker")]
        public string Symbol { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("locale")]
        public string Country { get; set; }
        
        [JsonPropertyName("primary_exchange")]
        public string Exchange { get; set; }
        
        [JsonPropertyName("market")]
        public string Market { get; set; }
        
        [JsonPropertyName("description")]
        public string Description { get; set; }
        
        [JsonPropertyName("homepage_url")]
        public string HomepageUrl { get; set; }
    }
}
