using System.ComponentModel.DataAnnotations.Schema;

namespace BoardGameModels
{
    public class BoardGameUser
    {
        public int Id { get; set; }
        [ForeignKey("BoardGame")]
        public int BoardGameId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
		public BoardGame? BoardGame { get; set; }
        public User? User { get; set; }
        public bool WishList { get; set; }
        public bool Owned { get; set; }
    }
}
