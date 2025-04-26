// Models/PlayerMatchItem.cs
namespace stratzclone.Server.Models
{
    public class PlayerMatchItem
    {
        // composite PK = (MatchId, SteamId, ItemSeq)
        public long   MatchId  { get; set; }
        public string SteamId  { get; set; } = null!;
        public int    ItemSeq  { get; set; }              // 0-based purchase order

        public int  ItemId        { get; set; }
        public int  PurchaseTime  { get; set; }           // seconds since game start
        public bool IsNeutral     { get; set; }
        public int? Charges       { get; set; }

        // back-reference (optional but handy)
        public PlayerMatch PlayerMatch { get; set; } = null!;
    }
}
