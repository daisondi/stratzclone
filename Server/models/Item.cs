using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace StratzClone.Server.Models
{
    public class Item
    {
        [JsonPropertyName("id")] public int ItemId { get; set; }           // PK
        [JsonPropertyName("name")] public string name { get; set; }
        [JsonPropertyName("url_image")] public string url_image { get; set; }

    }
}