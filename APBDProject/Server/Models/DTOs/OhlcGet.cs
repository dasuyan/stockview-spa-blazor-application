using System;
using System.Text.Json.Serialization;

namespace APBDProject.Server.Models.DTOs
{   //represents a singular instance of data needed to construct a OHLC chart.
    //used to get data from API.
    public class OhlcGet
    {   
        public DateTime Date { get; set; }
        
        [JsonPropertyName("o")]
        public double O { get; set; }
        [JsonPropertyName("c")]
        public double C { get; set; }
        [JsonPropertyName("h")]
        public double H { get; set; }
        [JsonPropertyName("l")]
        public double L { get; set; }
        [JsonPropertyName("v")]
        public double V { get; set; }
    }
}
