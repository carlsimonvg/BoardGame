namespace BoardGameModels
{
    public class Play
    {
        public int Id { get; set; }
        public BoardGame? BoardGame { get; set; }
        public List<Player>? Players { get; set; }
        public Player? Winner { get; set; }
        public int TimePlayed { get; set; }
        public bool Completed { get; set; }
        public DateTime? DatePlayed { get; set; }
    }
}