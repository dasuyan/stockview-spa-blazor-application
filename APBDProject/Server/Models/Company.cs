using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace APBDProject.Server.Models
{
    public class Company
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
        
        
        public virtual ICollection<Subscription> Users { get; set; }
        public virtual ICollection<Ohlc> Ohlcs { get; set; }
    }
}
