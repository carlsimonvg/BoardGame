using System.ComponentModel.DataAnnotations;

namespace BoardGameModels
{
    public class Player
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is required.")]
		public string? Name { get; set; }
		[Required(ErrorMessage = "This field is required.")]
		public int TotalWon { get; set; }
    }
}
