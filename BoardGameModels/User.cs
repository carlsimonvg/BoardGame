using System.ComponentModel.DataAnnotations;

namespace BoardGameModels
{
	public partial class User
	{
		public User()
		{
			RefreshTokens = new HashSet<RefreshToken>();
		}

		public int Id { get; set; }
		[Required(ErrorMessage = "This field is required.")]
		public string EmailAddress { get; set; }
		[Required(ErrorMessage = "This field is required.")]
		public string Password { get; set; }
		public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
	}
}