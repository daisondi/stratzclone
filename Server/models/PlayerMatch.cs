using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace stratzclone.Server.Models
{
    public class PlayerMatch
    {
        public long MatchId { get; set; }
        public string SteamId { get; set; } = null!;   // same type as Player.SteamId

        public int HeroId { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }
        public bool IsRadiant { get; set; }

        public int Item0Id { get; set; }
        public int Item1Id { get; set; }
        public int Item2Id { get; set; }
        public int Item3Id { get; set; }
        public int Item4Id { get; set; }
        public int Item5Id { get; set; }
        [JsonIgnore]
        public Match Match { get; set; } = null!;
        // public Player Player { get; set; } = null!;

        public ICollection<PlayerMatchItem> Items { get; set; }
            = new List<PlayerMatchItem>();
    }
}
