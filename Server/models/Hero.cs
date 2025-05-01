using System.Text.Json.Serialization;

namespace StratzClone.Server.Models
{
public class Hero
{
    [JsonPropertyName("id")]
    public int HeroId { get; set; }                  // numeric ID

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";           // internal name (npc_dota_hero_…)

    [JsonPropertyName("localized_name")]
    public string LocalizedName { get; set; } = "";  // Crystal Maiden, etc.

    [JsonPropertyName("pictureUrl")]
    public string PictureUrl { get; set; } = "";     // https://cdn.dota2.com/…
}
}