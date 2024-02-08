using BoardGameModels;

namespace BoardGameUI.Services
{
	public interface IUserService
	{
		public Task<UserWithToken> LoginAsync(User user);
		public Task<UserWithToken> RegisterUserAsync(User user);
		public Task<User> GetUserByAccessTokenAsync(string accessToken);
		public Task<UserWithToken> RefreshTokenAsync(RefreshRequest refreshRequest);
	}
}
