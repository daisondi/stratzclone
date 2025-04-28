using System.Collections.Generic;

namespace stratzclone.Server.Models
{
    public class PlayerMatch
    {
        public long   MatchId { get; set; }
        public string SteamId { get; set; } = null!;   // same type as Player.SteamId

        public int HeroId  { get; set; }
        public int Kills   { get; set; }
        public int Deaths  { get; set; }
        public int Assists { get; set; }
        public bool IsRadiant {get; set;}

        public Match  Match  { get; set; } = null!;
        public Player Player { get; set; } = null!;

        public ICollection<PlayerMatchItem> Items { get; set; }
            = new List<PlayerMatchItem>();
    }
}
