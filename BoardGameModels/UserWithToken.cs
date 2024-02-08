namespace BoardGameModels
{
	public class UserWithToken : User
	{
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }

		public UserWithToken()
		{
		}

		public UserWithToken(User user)
		{
			this.Id = user.Id;
			this.EmailAddress = user.EmailAddress;
		}
	}
}