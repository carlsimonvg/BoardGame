using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoardGameModels
{
    public class Play
    {
        public int Id { get; set; }
		[ForeignKey("BoardGame")]
		[Required(ErrorMessage = "This field is required 1.")]
		[DefaultValue(null)]
		public int? BoardGameId { get; set; }
		[ForeignKey("Player")]
		[Required(ErrorMessage = "This field is required 2.")]
		[DefaultValue(null)]
		public int? PlayerId { get; set; }
		public BoardGame? BoardGame { get; set; }
        public List<Player>? Players { get; set; }
        public Player? Winner { get; set; }
        [Required(ErrorMessage = "This field is required 3.")]
		public TimeSpan TimePlayed { get; set; }
		[Required(ErrorMessage = "This field is required 4.")]
		public bool? Completed { get; set; }
		[Required(ErrorMessage = "This field is required 5.")]
		public DateTime? DatePlayed { get; set; }
    }
}