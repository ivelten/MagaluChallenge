using Newtonsoft.Json;

namespace Magalu.Challenge.Data.External.Models
{
    public class MetadataModel
    {
        [JsonProperty(PropertyName = "page_number")]
        public int PageNumber { get; set; }

        [JsonProperty(PropertyName = "page_size")]
        public int PageSize { get; set; }
    }
}
