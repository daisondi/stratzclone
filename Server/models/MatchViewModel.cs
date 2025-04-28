public class MatchViewModel
{
    // Basic identifiers
    public long MatchId { get; set; }
    public DateTime StartTimeUtc { get; set; }
    public TimeSpan Duration { get; set; }
    
    // Hero
    public int HeroId { get; set; }
    public string HeroName { get; set; }
    public string HeroImageUrl { get; set; }
    
    // Outcome
    public bool IsWin { get; set; }
    public string ResultCssClass => IsWin ? "match-win" : "match-loss";

    // Performance
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public int Assists { get; set; }

}