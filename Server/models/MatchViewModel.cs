public class MatchViewModel
{
    // Basic identifiers
    public long MatchId { get; set; }
    public DateTime StartTimeUtc { get; set; }
    public TimeSpan Duration { get; set; }
    
    // Hero
    public int HeroId { get; set; }
    
    // Outcome
    public bool IsWin { get; set; } 
    public string ResultCssClass => IsWin ? "match-win" : "match-loss";

    // Performance
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public int Assists { get; set; }
    
        public int Item0url { get; set; }
        public int Item1url { get; set; }
        public int Item2url { get; set; }
        public int Item3url { get; set; }
        public int Item4url { get; set; }
        public int Item5url { get; set; }

}