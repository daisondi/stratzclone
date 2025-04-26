using System.Collections.Generic;

namespace stratzclone.Server.Models
{
    public class Match
    {
        public long MatchId { get; set; }           // PK
        public int  DurationSecs { get; set; }
        public bool DidRadiantWin { get; set; }

        public ICollection<PlayerMatch> PlayerMatches { get; set; }
            = new List<PlayerMatch>();
    }
}
