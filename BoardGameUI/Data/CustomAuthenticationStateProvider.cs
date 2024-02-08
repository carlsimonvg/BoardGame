using BoardGameModels;
using BoardGameUI.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Blazored.SessionStorage;

namespace BoardGameUI.Data
{
	public class CustomAuthenticationStateProvider : AuthenticationStateProvider
	{
		public ISessionStorageService _sessionStorageService { get; }
		public IUserService _userService { get; set; }
		private readonly HttpClient _httpClient;

		public CustomAuthenticationStateProvider(ISessionStorageService sessionStorageService,
			IUserService userService,
			HttpClient httpClient)
		{
			//throw new Exception("CustomAuthenticationStateProviderException");
			_sessionStorageService = sessionStorageService;
			_userService = userService;
			_httpClient = httpClient;
		}

		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			var accessToken = await _sessionStorageService.GetItemAsync<string>("accessToken");

			ClaimsIdentity identity;

			if (accessToken != null && accessToken != string.Empty)
			{
				User user = await _userService.GetUserByAccessTokenAsync(accessToken);
				identity = GetClaimsIdentity(user);
			}
			else
			{
				identity = new ClaimsIdentity();
			}

			var claimsPrincipal = new ClaimsPrincipal(identity);

			return await Task.FromResult(new AuthenticationState(claimsPrincipal));
		}

		public async Task MarkUserAsAuthenticated(UserWithToken user)
		{
			await _sessionStorageService.SetItemAsync("accessToken", user.AccessToken);
			await _sessionStorageService.SetItemAsync("refreshToken", user.RefreshToken);

			var identity = GetClaimsIdentity(user);

			var claimsPrincipal = new ClaimsPrincipal(identity);

			NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
		}

		public async Task MarkUserAsLoggedOut()
		{
			await _sessionStorageService.RemoveItemAsync("refreshToken");
			await _sessionStorageService.RemoveItemAsync("accessToken");

			var identity = new ClaimsIdentity();

			var user = new ClaimsPrincipal(identity);

			NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
		}

		private ClaimsIdentity GetClaimsIdentity(User user)
		{
			var claimsIdentity = new ClaimsIdentity();

			if (user.EmailAddress != null)
			{
				claimsIdentity = new ClaimsIdentity(new[]
								{
									new Claim(ClaimTypes.Name, user.EmailAddress)
								}, "apiauth_type");
			}

			return claimsIdentity;
		}
	}
}
