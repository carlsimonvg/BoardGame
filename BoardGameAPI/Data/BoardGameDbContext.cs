using Microsoft.EntityFrameworkCore;

namespace BoardGameAPI.Data
{
	public class BoardGameDbContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer()
			base.OnConfiguring(optionsBuilder);
		}
	}
}
