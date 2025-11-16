using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestaurantService.DadataApiModels
{
    public class DadataResponse
    {
        [JsonPropertyName("result")]
        public string Result { get; set; }
    
        [JsonPropertyName("region")]
        public string Region { get; set; }
    
        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("street")]
        public string Street { get; set; }
    
        [JsonPropertyName("house")]
        public string House { get; set; }
    
        [JsonPropertyName("qc")]
        public int QualityCheck { get; set; }
    }
}