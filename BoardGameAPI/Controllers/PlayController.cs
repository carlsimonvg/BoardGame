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
	public class PlayController : ControllerBase
    {
		private readonly BoardGameDbContext _context;

		public PlayController(BoardGameDbContext context)
		{
			_context = context;
		}

		// GET: api/Plays
		[HttpGet("GetPlays")]
		public async Task<ActionResult<IEnumerable<Play>>> GetPlays()
		{
			return  _context.Plays
				.Include(p => p.BoardGame)
				.Include(p => p.Winner)
				.ToList();
		}

		// GET: api/Plays/5
		[HttpGet("GetPlay/{id}")]
		public async Task<ActionResult<Play>> GetPlay(int id)
		{
			var play = await _context.Plays
											.Where(x => x.Id == id)
											.FirstOrDefaultAsync();

			if (play == null)
			{
				return NotFound();
			}

			return play;
		}

		// PUT: api/Plays/5
		[HttpPut("UpdatePlay/{id}")]
		public async Task<IActionResult> PutPlay(int id, Play play)
		{
			if (id != play.Id)
			{
				return BadRequest();
			}

			_context.Entry(play).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!PlayExists(id))
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

		// POST: api/Plays
		[HttpPost("CreatePlay")]
		public async Task<ActionResult<Play>> PostPlay(Play play)
		{
			_context.Plays.Add(play);
			await _context.SaveChangesAsync();

			return await Task.FromResult(play);
		}

		// DELETE: api/Plays/5
		[HttpDelete("DeletePlay/{id}")]
		public async Task<ActionResult<Play>> DeletePlay(int id)
		{
			var play = await _context.Plays.FindAsync(id);
			if (play == null)
			{
				return NotFound();
			}

			_context.Plays.Remove(play);
			await _context.SaveChangesAsync();

			return play;
		}

		private bool PlayExists(int id)
		{
			return _context.Plays.Any(e => e.Id == id);
		}
	}
}