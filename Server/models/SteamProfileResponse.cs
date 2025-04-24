// Models/SteamProfileResponse.cs
namespace stratzclone.Server.Models
{
    public class SteamProfileResponse
    {
        public SteamResponse response { get; set; }
    }

    public class SteamResponse
    {
        public PlayerDto[] players { get; set; }
    }

    public class PlayerDto
    {
        public string steamid { get; set; }
        public string personaname { get; set; }
        // add other fields here if you need them
    }
}
