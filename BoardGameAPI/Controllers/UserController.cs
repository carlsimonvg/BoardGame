using BoardGameAPI.Data;
using BoardGameModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BoardGameAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly BoardGameDbContext _context;
		private readonly JWTSettings _jwtSettings;

		public UserController(BoardGameDbContext context, IOptions<JWTSettings> jwtSettings)
		{
			_context = context;
			_jwtSettings = jwtSettings.Value;
		}

		// GET: api/Users
		[HttpGet("GetUsers")]
		public async Task<ActionResult<IEnumerable<User>>> GetUsers()
		{
			return await _context.Users.ToListAsync();
		}

		// GET: api/Users/5
		[HttpGet("GetUser/{id}")]
		public async Task<ActionResult<User>> GetUser(int id)
		{
			var user = await _context.Users.FindAsync(id);

			if (user == null)
			{
				return NotFound();
			}

			return user;
		}

		// GET: api/Users/5
		[HttpGet("GetUserDetails/{id}")]
		public async Task<ActionResult<User>> GetUserDetails(int id)
		{
			var user = await _context.Users.Where(u => u.Id == id)
											.FirstOrDefaultAsync();

			if (user == null)
			{
				return NotFound();
			}

			return user;
		}

		// POST: api/Users
		[HttpPost("Login")]
		public async Task<ActionResult<UserWithToken>> Login([FromBody] User user)
		{
			user = await _context.Users.Where(u => u.EmailAddress == user.EmailAddress
												&& u.Password == user.Password).FirstOrDefaultAsync();

			UserWithToken userWithToken = null;

			if (user != null)
			{
				RefreshToken refreshToken = GenerateRefreshToken();
				user.RefreshTokens.Add(refreshToken);
				await _context.SaveChangesAsync();

				userWithToken = new UserWithToken(user);
				userWithToken.RefreshToken = refreshToken.Token;
			}

			if (userWithToken == null)
			{
				return NotFound();
			}

			//sign your token here here..
			userWithToken.AccessToken = GenerateAccessToken(user.Id);
			return userWithToken;
		}

		// POST: api/Users
		[HttpPost("RegisterUser")]
		public async Task<ActionResult<UserWithToken>> RegisterUser([FromBody] User user)
		{
			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			//load role for registered user
			user = await _context.Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();

			UserWithToken userWithToken = null;

			if (user != null)
			{
				RefreshToken refreshToken = GenerateRefreshToken();
				user.RefreshTokens.Add(refreshToken);
				await _context.SaveChangesAsync();

				userWithToken = new UserWithToken(user);
				userWithToken.RefreshToken = refreshToken.Token;
			}

			if (userWithToken == null)
			{
				return NotFound();
			}

			//sign your token here here..
			userWithToken.AccessToken = GenerateAccessToken(user.Id);
			return userWithToken;
		}

		// GET: api/Users
		[HttpPost("RefreshToken")]
		public async Task<ActionResult<UserWithToken>> RefreshToken([FromBody] RefreshRequest refreshRequest)
		{
			User user = await GetUserFromAccessToken(refreshRequest.AccessToken);

			if (user != null && ValidateRefreshToken(user, refreshRequest.RefreshToken))
			{
				UserWithToken userWithToken = new UserWithToken(user);
				userWithToken.AccessToken = GenerateAccessToken(user.Id);

				return userWithToken;
			}

			return null;
		}

		// GET: api/Users
		[HttpPost("GetUserByAccessToken")]
		public async Task<ActionResult<User>> GetUserByAccessToken([FromBody] string accessToken)
		{
			User user = await GetUserFromAccessToken(accessToken);

			if (user != null)
			{
				return user;
			}

			return null;
		}

		private bool ValidateRefreshToken(User user, string refreshToken)
		{

			RefreshToken refreshTokenUser = _context.RefreshTokens.Where(rt => rt.Token == refreshToken)
												.OrderByDescending(rt => rt.ExpiryDate)
												.FirstOrDefault();

			if (refreshTokenUser != null && refreshTokenUser.UserId == user.Id
				&& refreshTokenUser.ExpiryDate > DateTime.UtcNow)
			{
				return true;
			}

			return false;
		}

		private async Task<User> GetUserFromAccessToken(string accessToken)
		{
			try
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

				var tokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false
				};

				SecurityToken securityToken;
				var principle = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out securityToken);

				JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;

				if (jwtSecurityToken != null && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
				{
					var userId = principle.FindFirst(ClaimTypes.Name)?.Value;

					return await _context.Users.Where(u => u.Id == Convert.ToInt32(userId)).FirstOrDefaultAsync();
				}
			}
			catch (Exception)
			{
				return new User();
			}

			return new User();
		}

		private RefreshToken GenerateRefreshToken()
		{
			RefreshToken refreshToken = new RefreshToken();

			var randomNumber = new byte[32];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(randomNumber);
				refreshToken.Token = Convert.ToBase64String(randomNumber);
			}
			refreshToken.ExpiryDate = DateTime.UtcNow.AddMonths(6);

			return refreshToken;
		}

		private string GenerateAccessToken(int userId)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, Convert.ToString(userId))
				}),
				Expires = DateTime.UtcNow.AddDays(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
				SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		// PUT: api/Users/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for
		// more details see https://aka.ms/RazorPagesCRUD.
		[HttpPut("UpdateUser/{id}")]
		public async Task<IActionResult> PutUser(int id, User user)
		{
			if (id != user.Id)
			{
				return BadRequest();
			}

			_context.Entry(user).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!UserExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// POST: api/Users
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for
		// more details see https://aka.ms/RazorPagesCRUD.
		[HttpPost("CreateUser")]
		public async Task<ActionResult<User>> PostUser(User user)
		{
			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetUser", new { id = user.Id }, user);
		}

		// DELETE: api/Users/5
		[HttpDelete("DeleteUser/{id}")]
		public async Task<ActionResult<User>> DeleteUser(int id)
		{
			var user = await _context.Users.FindAsync(id);
			if (user == null)
			{
				return NotFound();
			}

			_context.Users.Remove(user);
			await _context.SaveChangesAsync();

			return user;
		}

		private bool UserExists(int id)
		{
			return _context.Users.Any(e => e.Id == id);
		}
	}
}