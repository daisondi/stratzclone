// Models/Player.cs
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace stratzclone.Server.Models
{
    public class Player
    {
        [Key]
        public string SteamId { get; set; } = null!;      // Steam-64, e.g. "76561198012345678"
        public string? SteamId32 { get; set; }       // Steam-32, e.g. "12345678"
        public string? ProfilePictureUrl { get; set; }
        public string? DisplayName { get; set; }          // optional / nullable
        public string? Username    { get; set; }


    }
}
