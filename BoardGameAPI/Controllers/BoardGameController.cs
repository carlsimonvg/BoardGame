using System.Security.Claims;
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
	public class BoardGameController : ControllerBase
    {
		private readonly BoardGameDbContext _context;

		public BoardGameController(BoardGameDbContext context)
		{
			_context = context;
		}

		// GET: api/BoardGames
		[HttpGet("GetOwnedBoardGames")]
		public async Task<ActionResult<IEnumerable<BoardGame>>> GetOwnedBoardGames()
		{
			var userId = int.Parse(User.FindFirst(ClaimTypes.Name)?.Value);
			var hold = await _context.BoardGameUser
				.Where(bgu => bgu.UserId == userId && bgu.Owned)
				.Select(bgu => bgu.BoardGame)
				.ToListAsync();

			return Ok(hold);
		}

		// GET: api/BoardGames
		[HttpGet("GetBoardGames")]
		public async Task<ActionResult<IEnumerable<BoardGame>>> GetBoardGames()
		{
			return await _context.BoardGames.ToListAsync();
		}

		// GET: api/BoardGames
		[HttpGet("GetWishList")]
		public async Task<ActionResult<IEnumerable<BoardGame>>> GetWishList()
		{
			var userId = int.Parse(User.FindFirst(ClaimTypes.Name)?.Value);
			var hold = await _context.BoardGameUser
				.Where(bgu => bgu.UserId == userId && bgu.WishList)
				.Select(bgu => bgu.BoardGame)
				.ToListAsync();

			return Ok(hold);
		}

		// GET: api/BoardGames/5
		[HttpGet("GetBoardGame/{id}")]
		public async Task<ActionResult<BoardGame>> GetBoardGame(int id)
		{
			var boardGame = await _context.BoardGames
											.Where(pub => pub.Id == id)
											.FirstOrDefaultAsync();

			if (boardGame == null)
			{
				return NotFound();
			}

			return boardGame;
		}

		// PUT: api/BoardGames/5
		[HttpPut("UpdateBoardGame/{id}")]
		public async Task<IActionResult> PutBoardGame(int id, BoardGame boardGame)
		{
            if (id != boardGame.Id)
			{
				return BadRequest();
			}

			_context.Entry(boardGame).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!BoardGameExists(id))
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

		// POST: api/BoardGames
		[HttpPost("CreateBoardGame")]
		public async Task<ActionResult<BoardGame>> PostBoardGame(BoardGame boardGame)
        {
			_context.BoardGames.Add(boardGame);
			await _context.SaveChangesAsync();

			return await Task.FromResult(boardGame);
		}

        [HttpPost("BuyBoardGame")]
        public async Task<ActionResult<BoardGame>> PostBoardGameBuy(BoardGame boardGame)
        {
            if (!BoardGameExists(boardGame.Id))
            {
                _context.BoardGames.Add(boardGame);
            }

            await UpdateBoardGameUser(true, false, boardGame.Id);

            await _context.SaveChangesAsync();

            return await Task.FromResult(boardGame);
        }

        [HttpPost("WishListBoardGame")]
        public async Task<ActionResult<BoardGame>> PostBoardGameWishList(BoardGame boardGame)
        {
            if (!BoardGameExists(boardGame.Id))
            {
                _context.BoardGames.Add(boardGame);
            }

            await UpdateBoardGameUser(false, true, boardGame.Id);
			
	        await _context.SaveChangesAsync();

            return await Task.FromResult(boardGame);
        }

        // DELETE: api/BoardGames/5
        [HttpDelete("DeleteBoardGame/{id}")]
		public async Task<ActionResult<BoardGame>> DeleteBoardGame(int id)
		{
			var boardGame = await _context.BoardGames.FindAsync(id);
			if (boardGame == null)
			{
				return NotFound();
			}

			_context.BoardGames.Remove(boardGame);
			await _context.SaveChangesAsync();

			return boardGame;
		}

        private async Task UpdateBoardGameUser(bool owned, bool wishList, int boardGameId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.Name)?.Value);
            var boardGameUser = await _context.BoardGameUser.FirstOrDefaultAsync(e => e.BoardGameId == boardGameId && e.UserId == userId);

            if (boardGameUser != null)
            {
                boardGameUser.Owned = owned;
                boardGameUser.WishList = wishList;
                _context.Entry(boardGameUser).State = EntityState.Modified;
            }
            else
            {
                var entity = new BoardGameUser();
                entity.UserId = userId;
                entity.BoardGameId = boardGameId;
                entity.Owned = owned;
                entity.WishList = wishList;
                _context.BoardGameUser.Add(entity);
            }
        }

        private bool BoardGameExists(int id)
		{
			return _context.BoardGames.Any(e => e.Id == id);
		}
	}
}