namespace StratzClone.Server.Models
{
    public class MatchViewModel
{
    // Basic identifiers
    public long MatchId { get; set; }
    public DateTime StartTimeUtc { get; set; }
    public TimeSpan Duration { get; set; }
    
    // Hero
    public Hero Hero { get; set; }
    
    // Outcome
    public bool IsWin { get; set; } 
    public string ResultCssClass => IsWin ? "match-win" : "match-loss";

    // Performance
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public int Assists { get; set; }
    
        public Item Item0 { get; set; }
        public Item Item1 { get; set; }
        public Item Item2 { get; set; }
        public Item Item3 { get; set; }
        public Item Item4 { get; set; }
        public Item Item5 { get; set; }

}
}