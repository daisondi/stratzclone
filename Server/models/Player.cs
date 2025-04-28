// Models/Player.cs
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace stratzclone.Server.Models
{
    public class Player
    {
        [Key]
        public string SteamId { get; set; } = null!;      // Steam-64, e.g. "76561198012345678"

        public string? DisplayName { get; set; }          // optional / nullable
        public string? Username    { get; set; }


        // ── navigation to the link-table ───────────────
        public ICollection<PlayerMatch> PlayerMatches { get; set; } =
            new List<PlayerMatch>();
    }
}
