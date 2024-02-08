using BoardGameAPI.Data;
using BoardGameModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BoardGameAPI.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class PlayerController : ControllerBase
    {
		private readonly BoardGameDbContext _context;

		public PlayerController(BoardGameDbContext context)
		{
			_context = context;
		}

		// GET: api/Players
		[HttpGet("GetPlayers")]
		public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
		{
			return await _context.Players.ToListAsync();
		}

		// GET: api/Players/5
		[HttpGet("GetPlayer/{id}")]
		public async Task<ActionResult<Player>> GetPlayer(int id)
		{
			var player = await _context.Players
											.Where(x => x.Id == id)
											.FirstOrDefaultAsync();

			if (player == null)
			{
				return NotFound();
			}

			return player;
		}

		// PUT: api/Players/5
		[HttpPut("UpdatePlayer/{id}")]
		public async Task<IActionResult> PutPlayer(int id, Player player)
		{
			if (id != player.Id)
			{
				return BadRequest();
			}

			_context.Entry(player).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!PlayerExists(id))
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

		// POST: api/Players
		[HttpPost("CreatePlayer")]
		public async Task<ActionResult<Player>> PostPlayer(Player player)
		{
			_context.Players.Add(player);
			await _context.SaveChangesAsync();

			return await Task.FromResult(player);
		}

		// DELETE: api/Players/5
		[HttpDelete("DeletePlayer/{id}")]
		public async Task<ActionResult<Player>> DeletePlayer(int id)
		{
			var player = await _context.Players.FindAsync(id);
			if (player == null)
			{
				return NotFound();
			}

			_context.Players.Remove(player);
			await _context.SaveChangesAsync();

			return player;
		}

		private bool PlayerExists(int id)
		{
			return _context.Players.Any(e => e.Id == id);
		}
	}
}