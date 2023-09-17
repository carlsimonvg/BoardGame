namespace BoardGameModels
{
    public class Notification
    {
        public int Id { get; set; }
        public BoardGame? BoardGame { get; set; }
        public DateTime? PostPlayed { get; set; }
        public string? Message { get; set; }
        public string? Title { get; set; }
        public string? Language { get; set; }
    }
}