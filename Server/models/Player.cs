// Models/Player.cs
using System.ComponentModel.DataAnnotations;

namespace stratzclone.Server.Models
{
    public class Player
    {
        [Key]
        public string SteamId { get; set; }

        public string DisplayName { get; set; }

        public string Username { get; set; }
    }
}
