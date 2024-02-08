using BoardGameModels;
using Microsoft.EntityFrameworkCore;

namespace BoardGameAPI.Data
{
	public class BoardGameDbContext : DbContext
	{
		public BoardGameDbContext(DbContextOptions<BoardGameDbContext> options)
			: base(options)
		{
		}

		public DbSet<BoardGame> BoardGames { get; set; }
		public DbSet<Play> Plays { get; set; }
		public DbSet<Player> Players { get; set; }
		public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Notification> Notifications { get; set; }
        public DbSet<BoardGameUser> BoardGameUser { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer("Name=BoardGameDB");
			}
			//optionsBuilder.UseSqlServer(@"Data Source=RD05990-SIMONVG;Initial Catalog=BoardGame;Integrated Security=True;TrustServerCertificate=True");
			//base.OnConfiguring(optionsBuilder);
		}
	}
}
