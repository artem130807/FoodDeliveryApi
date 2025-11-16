using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IdentityService.DadataApiModels
{
    public class DadataResponce
    {
        [JsonPropertyName("source")]
        public string Source { get; set; }
    
        [JsonPropertyName("result")]
        public string Result { get; set; }
    
        [JsonPropertyName("city")]
        public string City { get; set; }
    
        [JsonPropertyName("region")]
        public string Region { get; set; }
    
        [JsonPropertyName("qc")]
        public int QualityCheck { get; set; }
    }
}